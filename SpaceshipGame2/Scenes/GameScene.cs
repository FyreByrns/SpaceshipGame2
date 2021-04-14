using PixelEngine;

namespace SpaceshipGame2.Scenes {
	class GameScene : Scene {
		public World.World world;

		public override void Update(float elapsed) {
			world.Update(owner, elapsed);
			world.Draw(owner);
		}

		public GameScene(SpaceGame owner) : base(owner) {
			world = new World.World((0, 0), (owner.ScreenWidth, owner.ScreenHeight));
		}
	}
}

