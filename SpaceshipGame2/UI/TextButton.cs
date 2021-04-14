
using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.UI {
	class TextButton : UIElement {
		public string text;

		public override void Draw(SpaceGame target) {
			base.Draw(target);

			target.FillRect(bounds.topLeft, bounds.bottomRight, currentColours.fill);
			target.DrawRect(bounds.topLeft, bounds.bottomRight, currentColours.outline);
			target.DrawText(bounds.topLeft + (1, 1), text, currentColours.foreground);
		}

		public TextButton(vf position, string text) : base(position, (text.Length * 8 + 2, 10)) {
			this.text = text;
		}
		public TextButton(vf position, vf size, string text) : base(position, size) {
			this.text = text;
		}
	}
}