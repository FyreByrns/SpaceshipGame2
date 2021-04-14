using PixelEngine;

namespace SpaceshipGame2.Scenes {
	class GameScene : Scene {
		public World.World world;

		public override void Update(float elapsed) {
			if (owner.inputManager.ActionPressed("cancel"))
				owner.currentScene = new MenuScene(owner);
			world.Update(owner, elapsed);
			world.Draw(owner);
		}

		public GameScene(SpaceGame owner) : base(owner) {
			world = new World.World((0, 0), (owner.ScreenWidth, owner.ScreenHeight));
		}
	}
}

