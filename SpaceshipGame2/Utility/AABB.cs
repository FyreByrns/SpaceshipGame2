
using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Utility {
	class AABB {
		public vf topLeft, bottomRight;
		public float Width => bottomRight.x - topLeft.x;
		public float Height => bottomRight.y - topLeft.y;

		public AABB AtOrigin => new AABB(topLeft - topLeft, bottomRight - topLeft);

		public AABB(vf topLeft, vf bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;
		}

		public bool Overlaps(AABB other) {
			return topLeft.x <= other.bottomRight.x && topLeft.y <= other.bottomRight.y
				&& bottomRight.x >= other.topLeft.x && bottomRight.y >= other.topLeft.y;
		}

		public bool ContainsPoint(vf point) {
			return topLeft.x <= point.x && topLeft.y <= point.y
				&& bottomRight.x > point.x && bottomRight.y > point.y;
		}

		/// <summary>
		/// Creates an AABB which contains both AABBs
		/// </summary>
		public AABB Combine(AABB other) {
			AABB result = new AABB(topLeft, bottomRight);

			if (other.topLeft.x < topLeft.x) {
				result.topLeft.x = other.topLeft.x;
			}
			if (other.bottomRight.x > bottomRight.x) {
				result.bottomRight.x = bottomRight.x;
			}

			if (other.topLeft.y < topLeft.y) {
				result.topLeft.y = other.topLeft.y;
			}
			if (other.bottomRight.y > bottomRight.y) {
				result.bottomRight.y = other.bottomRight.y;
			}

			return result;
		}
	}
}

