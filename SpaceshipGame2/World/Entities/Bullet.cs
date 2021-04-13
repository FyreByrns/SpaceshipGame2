using PixelEngine;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.World.Entities {
	class Bullet : Entity {
		float lifetime = 3;
		bool wantToDie = false;

		public override void Update(Game target, float elapsed) {
			base.Update(target, elapsed);

			lifetime -= elapsed;
			if (lifetime < 0) wantToDie = true;
		}

		public override void Draw(Game target, World world) {
			base.Draw(target, world);

			if (wantToDie) chunk.world.RegisterForDeletion(this);
		}

		public Bullet(vf position_w) : base(position_w) {
		}
	}
}

