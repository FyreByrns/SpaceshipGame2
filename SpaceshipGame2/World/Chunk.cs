using PixelEngine;
using SpaceshipGame2.Utility;
using System.Collections.Generic;

using vf = SpaceshipGame2.Utility.Vector2;
using vi = SpaceshipGame2.Utility.Vector2i;

namespace SpaceshipGame2.World {
	class Chunk {
		#region static
		public static int chunkSize = 10000;

		public static vf ChunkOrigin_w(vi chunkPos_c) => chunkPos_c * chunkSize;
		public static vi ChunkFromWorldPosition_c(vf position_w) => (System.Math.Floor(position_w.x / chunkSize), System.Math.Floor(position_w.y / chunkSize));

		static JavidRng chunkRng = new JavidRng();
		#endregion static

		#region instance
		public World world;
		public vi chunkPosition_c;
		public List<WorldObject> worldObjects;

		class ChunkLocalStar : WorldObject {
			public override void Draw(Game target, World world) {
				//int radius_s = (int)world.ScreenLength(1);
				vf position_s = world.ScreenPoint(position_w);
				//target.FillCircle(position_s, radius_s, Pixel.Presets.White);
				target.Draw(position_s, Pixel.Presets.White);
			}

			public override AABB GetBounds() {
				return new AABB(position_w - 0.5f, position_w + 0.5f);
			}

			public ChunkLocalStar(vf position_w) : base() {
				this.position_w = position_w;
				this.shouldBeSaved = false;
			}
		}

		public Chunk(World world, vi chunkPosition_c, params WorldObject[] worldObjects) {
			this.world = world;
			this.chunkPosition_c = chunkPosition_c;
			this.worldObjects = new List<WorldObject>(worldObjects);

			chunkRng.Seed((uint)(30 + chunkPosition_c.x * 65536 + chunkPosition_c.y));
			chunkRng.Next();
			for (int i = 0; i < 1000; i++)
				this.worldObjects.Add(new ChunkLocalStar(ChunkOrigin_w(chunkPosition_c) + (chunkRng.Next(0, chunkSize), chunkRng.Next(0, chunkSize))) { chunk = this });
		}
		#endregion instance
	}
}

