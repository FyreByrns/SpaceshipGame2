using PixelEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using vf = SpaceshipGame2.Vector2;

namespace SpaceshipGame2.World.Graphics {
	class MultiPolygon : Polygon {
		public Dictionary<string, Polygon> shapes;

		float lastRotation, lastScale;
		vf lastPosition_w;

		public override void Draw(Game target, World cam) {
			foreach (Polygon shape in shapes.Values) {
				shape.rotation -= lastRotation - rotation;
				shape.scale -= lastScale - scale;
				shape.position_w -= lastPosition_w - position_w;
				shape.Draw(target, cam);
			}

			lastPosition_w = position_w;
			lastRotation = rotation;
			lastScale = scale;
		}

		public override AABB GetBounds() {
			vf min = (float.MaxValue, float.MaxValue);
			vf max = (float.MinValue, float.MinValue);

			foreach (Polygon shape in shapes.Values) {
				AABB bounds = shape.GetBounds();
				if (bounds.topLeft.x < min.x) min.x = bounds.topLeft.x;
				if (bounds.topLeft.y < min.y) min.y = bounds.topLeft.y;
				if (bounds.bottomRight.x > max.x) max.x = bounds.bottomRight.x;
				if (bounds.bottomRight.y > max.y) max.y = bounds.bottomRight.y;
			}

			return new AABB(min, max);
		}

		public MultiPolygon() {
			shapes = new Dictionary<string, Polygon>();
		}

		struct ProcessingState {
			public List<vf> vertices;
			public string name;
			public Pixel colour;
			public float scale;

			public bool IsComplete() =>
				vertices.Count >= 3 &&
				!string.IsNullOrEmpty(name) &&
				colour != default;
		}

		static void Process(string d, ref ProcessingState state) {
			if (state.vertices == null) state.vertices = new List<vf>();
			if (state.scale == 0) state.scale = 1;

			Regex regex = new Regex(@".*?\(");
			string key = regex.Match(d).Value;
			key = key.Substring(0, key.Length - 1);

			regex = new Regex(@"\((.*?)\)");
			string value = regex.Match(d).Value;
			value = value.Substring(1, value.Length - 2);

			switch (key) {
				case "scale": {
						if (float.TryParse(value, out float sScaleFloat))
							state.scale = sScaleFloat;
					}
					break;

				case "v": {
						string[] vData = value.Split(',');

						vf result = default;
						bool onX = true;
						foreach (string vElement in vData) {
							if (float.TryParse(vElement, out float vElementFloat)) {
								if (onX) {
									result.x = vElementFloat;
									onX = false;
								} else result.y = vElementFloat;
							}
						}

						state.vertices.Add(result);
					}
					break;

				case "name": {
						state.name = value;
					}
					break;

				case "colour": {
						string[] cData = value.Split(',');
						byte[] cDataBytes = new byte[4];

						int i = 0;
						foreach (string cElement in cData) {
							if (byte.TryParse(cElement, out byte cElementByte)) {
								cDataBytes[i] = cElementByte;
							}
							i++;
						}
						if (i == 2) cDataBytes[3] = 255; // didn't get alpha value, default to fully opaque

						state.colour = new Pixel(cDataBytes[0], cDataBytes[1], cDataBytes[2], cDataBytes[3]);
					}
					break;
			}
		}
		public static MultiPolygon FromString(string data) {
			MultiPolygon polygon = new MultiPolygon();

			Regex regex = new Regex(@"[ \r\n\t]");
			data = regex.Replace(data, "");

			regex = new Regex(@"\[(.*?)\]");
			MatchCollection matches = regex.Matches(data);
			string[] splitData = new string[matches.Count];

			for (int i = 0; i < splitData.Length; i++) {
				splitData[i] = matches[i].Value.Substring(1, matches[i].Value.Length - 2);
			}

			foreach (string s in splitData) {
				regex = new Regex(@"\(?(.*?)\)");

				ProcessingState state = new ProcessingState();
				foreach (Match m in regex.Matches(s))
					Process(m.Value, ref state);

				if (state.IsComplete()) {
					polygon.shapes[state.name] = new Polygon(state.vertices.ToArray()) { colourOutline = state.colour, scale = state.scale };

					foreach (vf v in state.vertices) {
						System.Console.WriteLine($"{v.x},{v.y}");
					}
				}
			}

			return polygon;
		}
	}
}
