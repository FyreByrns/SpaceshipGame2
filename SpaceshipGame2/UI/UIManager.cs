using System.Collections.Generic;

namespace SpaceshipGame2.UI {
	class UIManager {
		SpaceGame owner;
		public List<UIElement> uiElements;
		public UIElement selected;

		public void Update() {
			foreach (UIElement element in uiElements)
				element.Update(owner);

			if (owner.inputManager.ActionPressed("up")) if(selected.up != null)selected = selected.up;
			if (owner.inputManager.ActionPressed("down")) if(selected.down != null)selected = selected.down;
			if (owner.inputManager.ActionPressed("left")) if(selected.left != null)selected = selected.left;
			if (owner.inputManager.ActionPressed("right")) if(selected.right != null)selected = selected.right;

			if (owner.inputManager.ActionPressed("confirm")) selected.Press();
		}

		public void Draw() {
			foreach (UIElement element in uiElements)
				element.Draw(owner);
		}

		public UIManager(SpaceGame owner) {
			this.owner = owner;
			uiElements = new List<UIElement>();
		}
	}
}