using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PixelEngine;
using SpaceshipGame2.UI;
using SpaceshipGame2.Utility;

/* ✔
[ ] manifest view
	[ ] load and edit
	[ ] copy
	[ ] delete
	[ ] rename
[ ] editor
	[ ] show all shapes
	[ ] move vertices
	[ ] add / remove vertices
	[ ] hide / show certain shapes
	[ ] rename shapes
	[ ] recolour shapes
	[ ] add / remove shapes
*/

namespace SpaceshipGame2.Scenes {
	class ShapeEditorScene : Scene {
		public UIManager uiManager;

		public override void Update(float elapsed) {
			owner.Clear(Pixel.Empty);

			if (owner.inputManager.ActionPressed("cancel"))
				owner.currentScene = new MenuScene(owner);

			uiManager.Update();
			uiManager.Draw();
		}

		public ShapeEditorScene(SpaceGame owner) : base(owner) {
			uiManager = new UIManager(owner);
			uiManager.uiElements.Add(new ShapeAssetList((10, 10), (100, 100)));
		}
	}

	class ShapeAssetList : ScrollContainer {

		public ShapeAssetList(Vector2 position, Vector2 size) : base(position, size) {
		}
	}
}
