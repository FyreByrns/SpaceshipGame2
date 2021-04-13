using PixelEngine;

using vf = SpaceshipGame2.Vector2;

namespace SpaceshipGame2.World {
	abstract class WorldObject {
		public vf position_w;
		public Chunk chunk;
		public bool shouldBeSaved = true;

		public abstract AABB GetBounds();
		public abstract void Draw(Game target, World world);
	}
}

