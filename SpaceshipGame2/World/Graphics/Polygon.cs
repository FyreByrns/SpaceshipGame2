using PixelEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using vf = SpaceshipGame2.Vector2;

namespace SpaceshipGame2.World.Graphics {
	class Polygon : WorldObject {
		public vf[] vertices;

		public float rotation = 0;
		public float scale = 1;

		public Pixel colourOutline;

		public override AABB GetBounds() {
			vf min, max;
			min = (float.MaxValue, float.MaxValue);
			max = (float.MinValue, float.MinValue);

			foreach (vf v in GetTransformedVertices()) {
				if (v.x < min.x) min.x = v.x;
				if (v.y < min.y) min.y = v.y;
				if (v.x > max.x) max.x = v.x;
				if (v.y > max.y) max.y = v.y;
			}

			return new AABB(min, max);
		}

		public override void Draw(Game target, World cam) {
			IEnumerable<vf> vertices_w = GetTransformedVertices();
			vf last_s = cam.ScreenPoint(vertices_w.Last());

			foreach (vf v in vertices_w) {
				vf current_s = cam.ScreenPoint(v);
				target.DrawLine(last_s, current_s, colourOutline);
				last_s = current_s;
			}
		}

		public IEnumerable<vf> GetRotatedVertices() {
			foreach (vf p in vertices) {
				vf result = (0, 0);

				float rot = rotation - (float)Math.PI / 2;

				float cos = (float)Math.Cos(rot);
				float sin = (float)Math.Sin(rot);

				result.x = scale * (p.x * cos - p.y * sin);
				result.y = scale * (p.x * sin + p.y * cos);

				yield return result;
			}
		}

		public IEnumerable<vf> GetTransformedVertices() {
			foreach (vf p in GetRotatedVertices())
				yield return p + position_w;
		}

		protected Polygon() { }
		public Polygon(params vf[] vertices) {
			if (vertices.Length < 3) throw new ArgumentException("Cannot create polygon with fewer than 3 vertices.");
			this.vertices = vertices;
		}
	}
}
