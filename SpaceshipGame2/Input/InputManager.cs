using PixelEngine;
using System.IO;
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

		/// <summary>
		/// Save input settings to disk
		/// </summary>
		public void Save() {
			List<string> saveLines = new List<string>();
			foreach (string action in bindings.Keys) {
				saveLines.Add($"{action}:{bindings[action]}");
			}

			File.WriteAllLines("input.ini", saveLines.ToArray());
		}

		/// <summary>
		/// Load input settings from disk
		/// </summary>
		public void Load() {
			if (!File.Exists("input.ini")) return; // no changes from default

			string[] lines = File.ReadAllLines("input.ini");
			foreach(string line in lines) {
				string[] data = line.Split(':');
				string action = data[0];
				string keyString = data[1];
				Key key = (Key)System.Enum.Parse(typeof(Key), keyString);
				Bind(action, key);
			}
		}

		public void Update() {

		}

		public InputManager(SpaceGame owner) {
			this.owner = owner;

			Bind("fire", Key.Space);
			Bind("cancel", Key.Escape);
			Bind("confirm", Key.Enter);
			Bind("up", Key.W);
			Bind("down", Key.S);
			Bind("left", Key.A);
			Bind("right", Key.D);
			Load();
		}
	}
}
