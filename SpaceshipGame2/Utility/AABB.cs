
using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.Utility {
	class AABB {
		public vf topLeft, bottomRight;
		public float width => bottomRight.x - topLeft.x;

		public AABB(vf topLeft, vf bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;
		}

		public bool Overlaps (AABB other) {
			return topLeft.x <= other.bottomRight.x && topLeft.y <= other.bottomRight.y
				&& bottomRight.x >= other.topLeft.x && bottomRight.y >= other.topLeft.y;
		}

		public bool ContainsPoint(vf point) {
			return topLeft.x <= point.x && topLeft.y <= point.y
				&& bottomRight.x > point.x && bottomRight.y > point.y;
		}
	}
}

