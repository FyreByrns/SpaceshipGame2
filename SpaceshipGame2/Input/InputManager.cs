using PixelEngine;
using System.Collections.Generic;

namespace SpaceshipGame2.Input {
	class InputManager {
		public SpaceGame owner;

		Dictionary<string, List<Key>> bindings = new Dictionary<string, List<Key>>();

		public void Bind(string action, Key key) {
			if (!bindings.ContainsKey(action))
				bindings[action] = new List<Key>();
			bindings[action].Add(key);
		}

		public void Unbind(string action, Key key) {
			if (bindings.ContainsKey(action)) {
				if (key == Key.Any) 
					bindings.Remove(action);
				else if (bindings[action].Contains(key))
					bindings[action].Remove(key);
			}
		}

		public bool ActionPressed(string action) {
			if (bindings.ContainsKey(action)) {
				foreach (Key key in bindings[action])
					if (owner.GetKey(key).Down) return true;
			}
			return false;
		}

		public void Update() {

		}

		public InputManager(SpaceGame owner) {
			this.owner = owner;

			Bind("up", Key.W);
			Bind("down", Key.S);
			Bind("left", Key.A);
			Bind("right", Key.D);
		}
	}
}
