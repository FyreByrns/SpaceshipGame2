using PixelEngine;

namespace SpaceshipGame2.Scenes {
	class GameScene : Scene {
		public World.World world;

		public override void Update(float elapsed) {
			if (owner.GetKey(Key.W).Down) world.cameraPosition_w.y--;
			if (owner.GetKey(Key.S).Down) world.cameraPosition_w.y++;
			if (owner.GetKey(Key.A).Down) world.cameraPosition_w.x--;
			if (owner.GetKey(Key.D).Down) world.cameraPosition_w.x++;

			world.Update(owner, elapsed);
			world.Draw(owner);
		}

		public GameScene(SpaceGame owner) : base(owner) {
			world = new World.World((0, 0), (owner.ScreenWidth, owner.ScreenHeight));
		}
	}
}

