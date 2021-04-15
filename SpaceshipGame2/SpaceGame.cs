using PixelEngine;
using SpaceshipGame2.Scenes;
using System.Runtime.InteropServices;

namespace SpaceshipGame2 {
	class SpaceGame : Game {
		public Scene currentScene;
		public Input.InputManager inputManager;

		public override void OnCreate() {
			base.OnCreate();
			//Sound.AudioManager.Start();
			IO.AssetManager.Start();
		}

		public override void OnDestroy() {
			base.OnDestroy();
			//Sound.AudioManager.Stop();
		}

		public override void OnUpdate(float elapsed) {
			base.OnUpdate(elapsed);

			inputManager.Update();
			currentScene.Update(elapsed);
		}

		public SpaceGame() {
			Construct(200, 200, 4, 4);
			inputManager = new Input.InputManager(this);
			currentScene = new MenuScene(this);

			//FreeConsole();
			Start();
		}

		[DllImport("kernel32.dll")]
		static extern void FreeConsole();
	}
}

