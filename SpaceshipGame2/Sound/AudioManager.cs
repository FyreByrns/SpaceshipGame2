using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;

namespace SpaceshipGame2.Sound {
	static class AudioManager {
		static ISoundEngine soundEngine;

		public static void Start() {
			soundEngine = new ISoundEngine();
		}

		public static void PlaySound(string path) {
			soundEngine.Play2D(path);
		}

		public static void Stop() {
		}
	}
}
