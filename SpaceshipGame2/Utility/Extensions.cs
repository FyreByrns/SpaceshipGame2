using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceshipGame2 {
	public static class Extensions {
		public static float Truncate(this float value, int digits) {
			double mult = Math.Pow(10.0, digits);
			double result = Math.Truncate(mult * value) / mult;
			return (float)result;
		}

		public static float TowardsButNotPass(this float value, float target, float by) {
			float temp = value;
			if (value > target) temp -= by;
			if (value < target) temp += by;

			if (value >= target && temp <= target) return target;
			if (value <= target && temp >= target) return target;
			return temp;
		}
	}
}
