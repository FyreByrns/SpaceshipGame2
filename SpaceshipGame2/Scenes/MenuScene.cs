using SpaceshipGame2.UI;
using PixelEngine;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Scenes {
	class MenuScene : Scene {
		UIManager uiManager;

		public override void Update(float elapsed) {
			uiManager.Update();
			uiManager.Draw();
		}

		public MenuScene(SpaceGame owner) : base(owner) {
			uiManager = new UIManager(owner);

			TextButton playButton = new TextButton((10, 10), "play");
			playButton.OnMouseClicked += PlayClicked;
			uiManager.uiElements.Add(playButton);

			TextButton quitButton = new TextButton(playButton.bottomLeft + (0, 1), "quit");
			quitButton.OnMouseClicked += QuitClicked;
			uiManager.uiElements.Add(quitButton);
		}

		private void PlayClicked(vf mouse, Mouse button) {
			Scene playScene = new GameScene(owner);
			owner.currentScene = playScene;
		}

		private void QuitClicked(vf mouse, Mouse button) {
			owner.Finish();
		}
	}
}
