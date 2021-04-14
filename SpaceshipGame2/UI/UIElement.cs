using SpaceshipGame2.Utility;
using PixelEngine;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.UI {
	abstract class UIElement {
		public enum UIElementState {
			None = 0,

			Up,
			Down,
			Hovered,
			Disabled
		}

		#region events
		public delegate void MouseEventHandler(UIElement sender, vf mouse, Mouse button);
		public event MouseEventHandler OnMouseHover;
		public event MouseEventHandler OnMouseDown;
		public event MouseEventHandler OnMouseHeld;
		public event MouseEventHandler OnMouseUp;
		public event MouseEventHandler OnMouseClicked;
		#endregion events

		#region mouse state
		bool[] wasPressed;
		#endregion mouse state

		public AABB bounds;
		public UIElementState state;

		public vf topLeft => bounds.topLeft;
		public vf bottomLeft => (bounds.topLeft.x, bounds.bottomRight.y);
		public vf topRight => (bounds.bottomRight.x, bounds.topLeft.y);
		public vf bottomRight => bounds.bottomRight;

		public UIColourCollection upColours, downColours, hoverColours, disabledColours;
		public UIColourCollection currentColours;

		public virtual void Update(SpaceGame target) {
			switch (state) {
				case UIElementState.Up:
					currentColours = upColours;
					break;
				case UIElementState.Down:
					currentColours = downColours;
					break;
				case UIElementState.Hovered:
					currentColours = hoverColours;
					break;
				case UIElementState.Disabled:
					currentColours = disabledColours;
					break;
			}

			// if the element is disabled, don't update
			if (state == UIElementState.Disabled) {
				currentColours = disabledColours;
				return;
			} else if (state == UIElementState.Hovered) {
				state = UIElementState.Up;
			}

			vf mouse = (target.MouseX, target.MouseY);
			// if the mouse is over the element
			if (bounds.ContainsPoint(mouse)) {
				// check all mouse buttons
				bool anyTrue = false;
				for (Mouse check = Mouse.Left; check <= Mouse.Right; check++) {
					if (target.GetMouse(check).Down) {
						anyTrue = true;
						if (!wasPressed[(int)check])
							OnMouseDown?.Invoke(this, mouse, check);
						else
							OnMouseHeld?.Invoke(this, mouse, check);

						wasPressed[(int)check] = true;

					} else {
						if (wasPressed[(int)check]) {
							OnMouseUp?.Invoke(this, mouse, check);
							OnMouseClicked?.Invoke(this, mouse, check);
						}

						wasPressed[(int)check] = false;
					}
				}
				if (!anyTrue) // if none were pressed and the mouse is over the element, the element is hovered
					OnMouseHover?.Invoke(this, mouse, Mouse.None);
			}
		}

		public virtual void Draw(SpaceGame target) {
			target.DrawRect(bounds.topLeft, bounds.bottomRight, Pixel.Presets.Brown);
		}

		protected UIElement(vf position, vf size) {
			wasPressed = new bool[3];

			state = UIElementState.Up;
			upColours = UIColourCollection.DefaultUp;
			downColours = UIColourCollection.DefaultDown;
			hoverColours = UIColourCollection.DefaultHover;
			disabledColours = UIColourCollection.DefaultDisabled;

			OnMouseHover += UIElement_OnMouseHover;
			OnMouseDown += UIElement_OnMouseDown;
			OnMouseUp += UIElement_OnMouseUp;

			bounds = new AABB(position, position + size);
		}

		private void UIElement_OnMouseUp(UIElement sender, vf mouse, Mouse button) {
			state = UIElementState.Up;
		}
		private void UIElement_OnMouseDown(UIElement sender, vf mouse, Mouse button) {
			state = UIElementState.Down;
		}
		private void UIElement_OnMouseHover(UIElement sender, vf mouse, Mouse button) {
			state = UIElementState.Hovered;
		}
	}
}