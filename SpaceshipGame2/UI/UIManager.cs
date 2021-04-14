using System.Collections.Generic;

namespace SpaceshipGame2.UI {
	class UIManager {
		SpaceGame owner;
		public List<UIElement> uiElements;

		public void Update() {
			foreach (UIElement element in uiElements)
				element.Update(owner);
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