using PixelEngine;

namespace SpaceshipGame2.UI {
	struct UIColourCollection {
		public Pixel outline, fill, foreground;

		public static readonly UIColourCollection DefaultUp = new UIColourCollection { outline = new Pixel(200, 200, 200), fill = new Pixel(100, 100, 100), foreground = new Pixel(255, 255, 255) };
		public static readonly UIColourCollection DefaultDown = new UIColourCollection { outline = DefaultUp.fill, fill = DefaultUp.outline, foreground = DefaultUp.foreground };
		public static readonly UIColourCollection DefaultHover = new UIColourCollection { outline = new Pixel(220, 220, 220), fill = new Pixel(120, 120, 120), foreground = DefaultUp.foreground };
		public static readonly UIColourCollection DefaultDisabled = new UIColourCollection { outline = new Pixel(100, 100, 100), fill = new Pixel(10, 10, 10), foreground = new Pixel(200, 200, 200) };
	}
}