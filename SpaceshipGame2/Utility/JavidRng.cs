
// Algorithm is borrowed from 
// https://gist.github.com/dandistine/4609751d1499bef2e4cdea1cc5189352
// (original from https://lemire.me/blog/2019/03/19/the-fastest-conventional-random-number-generator-that-can-pass-big-crush/)
// Ported to C# and probably made much worse.

namespace SpaceshipGame2 {
	class JavidRng {
		uint state;

		public void Seed(uint seed) {
			state = seed;
		}

		public uint Next() {
			ulong tmp;
			state += 0xE120FC15;

			tmp = (ulong)state * 0x4A39B70D;
			uint m1 = (uint)((tmp >> 32) ^ tmp);
			tmp = (ulong)m1 * 0x12FAD5C9;
			uint m2 = (uint)((tmp >> 32) ^ tmp);
			return m2;
		}

		public int Next(int min, int max) {
			uint internalResult = Next();

			return (int)((internalResult / (double)uint.MaxValue) * (max - min) + min);
		}

		public JavidRng() { }
		public JavidRng(uint state) {
			this.state = state;
		}
	}
}
