using PixelEngine;
using SpaceshipGame2.UI;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Scenes {
	class SettingsScene : Scene {
		UIManager uiManager;

		#region rebinding
		bool rebinding;
		string rebindingAction;
		#endregion rebinding

		public override void Update(float elapsed) {
			owner.Clear(Pixel.Presets.Black);

			if (rebinding) {
				owner.DrawText(new Point(10, 10), $"binding {rebindingAction}", Pixel.Presets.White);

				for (Key key = Key.A; key < Key.Any; key++) {
					if (owner.GetKey(key).Pressed) {
						owner.inputManager.Bind(rebindingAction, key);

						rebinding = false;
					}
				}
			} else {
				uiManager.Update();
				uiManager.Draw();
			}
		}

		public SettingsScene(SpaceGame owner) : base(owner) {
			uiManager = new UIManager(owner);
			TextButton backButton = new TextButton((10, 10), "back");
			backButton.OnMouseClicked += BackClicked;
			uiManager.uiElements.Add(backButton);
			uiManager.selected = backButton;

			TextButton saveButton = new TextButton(backButton.topRight + (1, 0), "save");
			saveButton.OnMouseClicked += SaveClicked;
			uiManager.uiElements.Add(saveButton);
			saveButton.left = backButton;
			backButton.right = saveButton;

			TextButton lastButton = backButton;
			foreach (string action in owner.inputManager.bindings.Keys) {
				TextButton actionButton = new RebindButton(lastButton.bottomLeft + (0, 1), (180, 10), action);
				actionButton.OnMouseClicked += RebindClicked;
				uiManager.uiElements.Add(actionButton);
				lastButton.down = actionButton;
				actionButton.up = lastButton;
				lastButton = actionButton;
			}

			foreach (UIElement element in uiManager.uiElements)
				element.OnMouseHover += AnyElementHover;
		}

		UIElement lastSender = null;
		private void AnyElementHover(UIElement sender, vf mouse, Mouse button) {
			//if (sender != lastSender) {
			//	try {
			//		Sound.AudioManager.PlaySound(IO.AssetManager.GetSound("ui-pop"));
			//	} catch { }
			//}
			//lastSender = sender;
		}

		private void SaveClicked(UIElement sender, vf mouse, Mouse button) {
			owner.inputManager.Save();
		}

		private void RebindClicked(UIElement sender, vf mouse, Mouse button) {
			rebinding = true;
			rebindingAction = (sender as RebindButton).action;
		}

		private void BackClicked(UIElement sender, vf mouse, Mouse button) {
			owner.currentScene = new MenuScene(owner);
		}

		class RebindButton : TextButton {
			public string action;

			public override void Update(SpaceGame target) {
				base.Update(target);

				text = $"{action}: {target.inputManager.bindings[action]}";
			}

			public RebindButton(vf position, vf size, string action) : base(position, size, "") {
				this.action = action;
			}
		}
	}
}
