using System;

namespace SpaceshipGame2.Utility {
	public static class Extensions {
		public static float Truncate(this float value, int digits) {
			double mult = Math.Pow(10.0, digits);
			double result = Math.Truncate(mult * value) / mult;
			return (float)result;
		}

		public static float Constrain (this float value, float min, float max) {
			if(value < min) {
				return min;
			}
			if(value > max) {
				return max;
			}
			return value;
		}

		public static float Map(this float value, float oldMin, float oldMax, float newMin, float newMax) {
			return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
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
