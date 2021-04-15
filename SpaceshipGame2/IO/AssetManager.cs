using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpaceshipGame2.World.Graphics;

namespace SpaceshipGame2.IO {
	static class AssetManager {
		public static string shapeManifestPath = "assets/manifest-shape.ini";
		public static string soundManifestPath = "assets/manifest-sound.ini";

		static Dictionary<string, MultiPolygon> shapes = new Dictionary<string, MultiPolygon>();
		static Dictionary<string, string> sounds = new Dictionary<string, string>();

		public static MultiPolygon GetShape(string name) {
			if (shapes.ContainsKey(name))
				return shapes[name];
			return null;
		}

		public static string GetSound(string name) {
			if (sounds.ContainsKey(name))
				return sounds[name];
			return null;
		}

		public static void Start() {
			if (!Directory.Exists("assets"))
				Directory.CreateDirectory("assets");
			if (!File.Exists(shapeManifestPath))
				File.Create(shapeManifestPath);
			if (!File.Exists(soundManifestPath))
				File.Create(soundManifestPath);

			// load shapes
			string[] shapeManifest = File.ReadAllLines(shapeManifestPath);
			foreach(string line in shapeManifest) {
				// ignore comments
				if (line.StartsWith("#")) continue;

				string[] lineData = line.Split(':');
				string name = lineData[0];
				string path = lineData[1];
				shapes[name] = MultiPolygon.FromString(File.ReadAllText(path));
			}

			// load sounds
			string[] soundManifest = File.ReadAllLines(soundManifestPath);
			foreach(string line in soundManifest) {
				// ignore comments
				if (line.StartsWith("#")) continue;

				string[] lineData = line.Split(':');
				string name = lineData[0];
				string path = lineData[1];
				sounds[name] = path;
			}
		}
	}
}
