using PixelEngine;
using System.Collections.Generic;

using vf = SpaceshipGame2.Vector2;
using vi = SpaceshipGame2.Vector2i;

namespace SpaceshipGame2 {
	class World : IUpdateable {
		#region vars camera
		public vf cameraPosition_w = (0, 0);
		public vf cameraSize_s;
		public float cameraZoom = 1;
		public bool cameraClear = true;
		public bool cameraDrawDebug = false;
		public Pixel cameraClearColour = Pixel.Presets.Black;
		vf cameraPanStart;
		#endregion vars camera

		public List<Entity> entities;
		public List<Entity> deadEntities;
		List<vf> stars_w = new List<vf>();

		float shootTimer = 0;
		public void Update(Game target, float elapsed) {
			if (entities.Count == 0)
				entities.Add(new Entity((0, 0)));

			if (target.GetKey(Key.Up).Down) entities[0].ApplyForceAtAngle(1, entities[0].rotation);
			if (target.GetKey(Key.Down).Down) entities[0].ApplyForceAtAngle(-1, entities[0].rotation);
			if (target.GetKey(Key.Left).Down) entities[0].ApplyRotationalForce(-0.1f);
			if (target.GetKey(Key.Right).Down) entities[0].ApplyRotationalForce(0.1f);

			shootTimer -= elapsed;
			if (target.GetKey(Key.Space).Down && shootTimer <= 0) {
				entities.Add(new Bullet(vf.Along(entities[0].position_w, 20, -entities[0].rotation + (float)System.Math.PI / 2)));
				entities[entities.Count - 1].rotation = entities[0].rotation;
				entities[entities.Count - 1].vel = entities[0].vel + vf.Along(default, 1, -entities[0].rotation + (float)System.Math.PI / 2);
				entities[entities.Count - 1].graphics = MultiPolygon.FromString("[name(bullet)v(-0.2,-0.5)v(0.2,-0.5)v(0.2,0.5)v(-0.2,0.5)colour(0,255,0)scale(10)]");
				shootTimer = 0.1f;
			}

			CameraCenterOnEntity(entities[0]);
			CameraZoomBy(1 + (float)target.MouseScroll / 10, cameraSize_s / 2);

			foreach (Entity e in entities)
				e.Update(target, elapsed);
		}

		public void Draw(Game target) {
			if (cameraClear) target.Clear(cameraClearColour);

			AABB screenBounds = new AABB(cameraPosition_w, cameraPosition_w + (WorldLength(cameraSize_s.x), WorldLength(cameraSize_s.y)));
			int rendered = 0;

			foreach (vf star_w in stars_w) {
				if (screenBounds.ContainsPoint(star_w)) {
					vf star_s = ScreenPoint(star_w);
					target.Draw(star_s, Pixel.Presets.White);
					rendered++;
				}
			}

			foreach (WorldObject e in entities) {
				AABB bnds = e.GetBounds();
				bnds.topLeft = ScreenPoint(bnds.topLeft);
				bnds.bottomRight = ScreenPoint(bnds.bottomRight);

				//if (screenBounds.Overlaps(bnds)) {
				e.Draw(target, this);
				//}
				rendered++;

				if (cameraDrawDebug) {
					target.DrawRect(ScreenPoint(e.GetBounds().topLeft), ScreenPoint(e.GetBounds().bottomRight), Pixel.Presets.Lavender);
					string posString = $"{e.position_w.x.Truncate(2)},{e.position_w.y.Truncate(2)}";
					target.DrawText(ScreenPoint(e.GetBounds().topLeft - (WorldLength(posString.Length * 4), WorldLength(8)) + (e.GetBounds().width / 2, 0)), posString, Pixel.Presets.Apricot);
				}
			}

			if (cameraDrawDebug) {
				target.DrawText(new Point(8, 8), $"{rendered}", Pixel.Presets.White);
				target.DrawText(new Point(0, 0), $"{screenBounds.topLeft.x},{screenBounds.topLeft.y}", Pixel.Presets.DarkCyan);
			}

			foreach (Entity e in deadEntities)
				entities.Remove(e);
			deadEntities.Clear();
		}

		#region methods camera
		public void CameraMove(vf amount_w) =>
			cameraPosition_w -= amount_w / cameraZoom;
		public void CameraCenterOn(vf location_w) =>
			cameraPosition_w = location_w - cameraSize_s / cameraZoom / 2;
		public void CameraCenterOnEntity(Entity e) =>
			CameraCenterOn(e.position_w);

		public float WorldLength(float length) =>
			length * 1 / cameraZoom;
		public float ScreenLength(float length) =>
			length * cameraZoom;

		/// <summary>
		/// Screen space -> World space
		/// </summary>
		public vf WorldPoint(vf screenPoint) =>
			(screenPoint / cameraZoom) + cameraPosition_w;
		/// <summary>
		/// World space -> Screen Space
		/// </summary>
		public vf ScreenPoint(vf worldPoint) =>
			(worldPoint - cameraPosition_w) * cameraZoom;

		public void CameraStartPan(vf loc) =>
			cameraPanStart = loc;
		public void CameraContinuePan(vf loc) {
			CameraMove(loc - cameraPanStart);
			cameraPanStart = loc;
		}

		public void CameraZoomBy(float amount, vf focus_s) {
			vf start_w = WorldPoint(focus_s);
			cameraZoom *= amount;
			vf end_w = WorldPoint(focus_s);
			cameraPosition_w += start_w - end_w;
		}
		#endregion methods camera

		public World(vf position_w, vf size_s) {
			cameraPosition_w = position_w;
			cameraSize_s = size_s;

			entities = new List<Entity>();
			deadEntities = new List<Entity>();

			System.Random r = new System.Random(30);
			for (int i = 0; i < 10000; i++)
				stars_w.Add((r.Next(-10000, 10000), r.Next(-10000, 10000)));
		}
	}

	class Chunk {
		#region static
		public static Dictionary<vi, Chunk> chunks = new Dictionary<vi, Chunk>();
		public static int chunkSize = 200;

		public static vf ChunkOrigin_w(vi chunkPos_c) => chunkPos_c * chunkSize;
		public static vi ChunkFromWorldPosition(vf position_w) => position_w / chunkSize;

		public static void UpdateInhabitedChunk(WorldObject worldObject) {
			worldObject.chunk.objects.Remove(worldObject);

			vi inhabitedChunk_c = ChunkFromWorldPosition(worldObject.position_w);
			if (!chunks.ContainsKey(inhabitedChunk_c))
				chunks[inhabitedChunk_c] = new Chunk(inhabitedChunk_c);

			chunks[inhabitedChunk_c].objects.Add(worldObject);
			worldObject.chunk = chunks[inhabitedChunk_c];
		}
		#endregion static

		#region instance
		public vi chunkPosition_c;
		public List<WorldObject> objects;

		public Chunk(vi chunkPosition_c, params WorldObject[] objects) {
			this.chunkPosition_c = chunkPosition_c;
			this.objects = new List<WorldObject>(objects);
		}
		#endregion instance
	}
}

