using System.Collections.Generic;

namespace SpaceshipGame2.UI {
	class UIManager {
		SpaceGame owner;
		public List<UIElement> uiElements;
		public UIElement selected;

		public void Update() {
			foreach (UIElement element in uiElements) {
				element.Update(owner);
				if (element.state == UIElement.UIElementState.Hovered)
					selected = element;
			}

			if (selected != null) {
				if (owner.inputManager.ActionDown("up"))	if (selected.up != null)	selected = selected.up;
				if (owner.inputManager.ActionDown("down"))	if (selected.down != null)	selected = selected.down;
				if (owner.inputManager.ActionDown("left"))	if (selected.left != null)	selected = selected.left;
				if (owner.inputManager.ActionDown("right")) if (selected.right != null) selected = selected.right;

				if (owner.inputManager.ActionDown("confirm")) selected?.Press();
			}
		}

		public void Draw() {
			if (selected != null) selected.state = UIElement.UIElementState.Hovered;
			foreach (UIElement element in uiElements)
				element.Draw(owner);
		}

		public UIManager(SpaceGame owner) {
			this.owner = owner;
			uiElements = new List<UIElement>();
		}
	}
}