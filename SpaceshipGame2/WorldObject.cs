using PixelEngine;

using vf = SpaceshipGame2.Vector2;

namespace SpaceshipGame2 {
	abstract class WorldObject {
		public vf position_w;
		public Chunk chunk;

		public abstract AABB GetBounds();
		public abstract void Draw(Game target, World world);
	}
}

