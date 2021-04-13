using PixelEngine;
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

	abstract class Scene {
		public SpaceGame owner;
		public abstract void Update(float elapsed);

		public Scene(SpaceGame owner) {
			this.owner = owner;
		}
	}
}

