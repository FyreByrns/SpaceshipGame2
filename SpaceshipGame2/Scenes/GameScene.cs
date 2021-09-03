using PixelEngine;
using SpaceshipGame2.UI;
using SpaceshipGame2.Utility;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Scenes {
	class GameScene : Scene {
		public World.World world;
		public UIManager uiManager;

		public override void Update(float elapsed) {
			if (owner.inputManager.ActionPressed("cancel"))
				owner.currentScene = new MenuScene(owner);
			world.Update(owner, elapsed);
			world.Draw(owner);

			uiManager.Update();
			uiManager.Draw();
		}

		public GameScene(SpaceGame owner) : base(owner) {
			world = new World.World((0, 0), (owner.ScreenWidth, owner.ScreenHeight));

			uiManager = new UIManager(owner);
			uiManager.uiElements.Add(new HUD(world.player as World.Entities.Player));
		}
	}
}

