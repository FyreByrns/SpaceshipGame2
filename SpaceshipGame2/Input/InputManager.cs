using PixelEngine;
using System.Collections.Generic;

namespace SpaceshipGame2.Input {
	class InputManager {
		public SpaceGame owner;

		public Dictionary<string, Key> bindings = new Dictionary<string, Key>();

		public void Bind(string action, Key key) {
			bindings[action] = key;
		}

		public void Unbind(string action) {
			if (bindings.ContainsKey(action))
				bindings.Remove(action);
		}

		public bool ActionPressed(string action) {
			if (bindings.ContainsKey(action)) {
				if (owner.GetKey(bindings[action]).Down)
					return true;
			}
			return false;
		}

		public void Update() {

		}

		public InputManager(SpaceGame owner) {
			this.owner = owner;

			Bind("cancel", Key.Escape);
			Bind("confirm", Key.Enter);
			Bind("up", Key.W);
			Bind("down", Key.S);
			Bind("left", Key.A);
			Bind("right", Key.D);
		}
	}
}
