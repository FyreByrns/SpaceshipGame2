using PixelEngine;
using System.Runtime.InteropServices;

namespace SpaceshipGame2 {
	class SpaceGame : Game {
		public World world;

		public override void OnCreate() {
			base.OnCreate();

			world = new World((0, 0), (ScreenWidth, ScreenHeight));
		}

		public override void OnUpdate(float elapsed) {
			base.OnUpdate(elapsed);

			if (GetKey(Key.W).Down) world.cameraPosition_w.y--;
			if (GetKey(Key.S).Down) world.cameraPosition_w.y++;
			if (GetKey(Key.A).Down) world.cameraPosition_w.x--;
			if (GetKey(Key.D).Down) world.cameraPosition_w.x++;

			world.Update(this, elapsed);
			world.Draw(this);
		}

		public SpaceGame() {
			Construct(200, 200, 4, 4);
			FreeConsole();
			Start();
		}

		[DllImport("kernel32.dll")]
		static extern void FreeConsole();
	}
}

