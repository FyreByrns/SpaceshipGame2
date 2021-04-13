using PixelEngine;
using SpaceshipGame2.Scenes;
using System.Runtime.InteropServices;

namespace SpaceshipGame2 {
	class SpaceGame : Game {
		public Scene currentScene;

		public override void OnCreate() {
			base.OnCreate();

		}

		public override void OnUpdate(float elapsed) {
			base.OnUpdate(elapsed);

			currentScene.Update(elapsed);
		}

		public SpaceGame() {
			Construct(200, 200, 4, 4);
			currentScene = new GameScene(this);

			FreeConsole();
			Start();
		}

		[DllImport("kernel32.dll")]
		static extern void FreeConsole();
	}
}

