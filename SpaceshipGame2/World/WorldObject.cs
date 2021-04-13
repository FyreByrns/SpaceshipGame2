using PixelEngine;
using SpaceshipGame2.Utility;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.World {
	abstract class WorldObject {
		public vf position_w;
		public Chunk chunk;
		public bool shouldBeSaved = true;

		public abstract AABB GetBounds();
		public abstract void Draw(Game target, World world);
	}
}

