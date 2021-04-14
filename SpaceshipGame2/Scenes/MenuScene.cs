using SpaceshipGame2.UI;
using PixelEngine;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Scenes {
	class MenuScene : Scene {
		UIManager uiManager;

		public override void Update(float elapsed) {
			owner.Clear(Pixel.Presets.Black);
			uiManager.Update();
			uiManager.Draw();
		}

		public MenuScene(SpaceGame owner) : base(owner) {
			uiManager = new UIManager(owner);

			TextButton playButton = new TextButton((10, 10), "play");
			playButton.OnMouseClicked += PlayClicked;
			uiManager.uiElements.Add(playButton);

			TextButton settingsButton = new TextButton(playButton.bottomLeft + (0, 1), "settings");
			settingsButton.OnMouseClicked += SettingsClicked;
			uiManager.uiElements.Add(settingsButton);

			TextButton quitButton = new TextButton(settingsButton.bottomLeft + (0, 1), "quit");
			quitButton.OnMouseClicked += QuitClicked;
			uiManager.uiElements.Add(quitButton);
		}

		private void PlayClicked(UIElement sender, vf mouse, Mouse button) {
			Scene playScene = new GameScene(owner);
			owner.currentScene = playScene;
		}
		
		private void SettingsClicked(UIElement sender, vf mouse, Mouse button) {
			Scene settingsScene = new SettingsScene(owner);
			owner.currentScene = settingsScene;
		}

		private void QuitClicked(UIElement sender, vf mouse, Mouse button) {
			owner.Finish();
		}
	}
}
