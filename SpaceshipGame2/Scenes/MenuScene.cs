namespace SpaceshipGame2.Scenes {
	class MenuScene : Scene{
		public override void Update(float elapsed) {
			owner.DrawCircle(new PixelEngine.Point(10, 10), 10, PixelEngine.Pixel.Presets.Blue);
			if (owner.GetKey(PixelEngine.Key.Space).Pressed) owner.currentScene = new GameScene(owner);
		}

		public MenuScene (SpaceGame owner) : base(owner) {

		}
	}
}
