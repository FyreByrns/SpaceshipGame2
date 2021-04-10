namespace SpaceshipGame2 {
	public struct Vector2i {
		public int x, y;

		public int Length2 => x * x + y * y;
		public int Length => (int)System.Math.Sqrt(Length2);

		public Vector2i RawNormal =>
			new Vector2i(y, -x);
		public Vector2i Normal =>
			RawNormal.Normalized();

		public Vector2i Normalized() {
			Vector2i result = new Vector2i();

			if (Length > 0) {
				result.x = x / Length;
				result.y = y / Length;
			}

			return result;
		}

		public int DotProduct(Vector2i other) =>
				DotProduct(this, other);

		public Vector2i Project(Vector2i onto) =>
				Project(this, onto);

		public static int DotProduct(Vector2i va, Vector2i vb) =>
				 va.x * vb.x + va.y * vb.y;

		public static Vector2i Project(Vector2i v, Vector2i onto) {
			int dp = DotProduct(v, onto);
			return onto * dp;
		}

		public static Vector2i Along(Vector2i origin, int distance, int rotation) =>
			new Vector2i((int)(origin.x + System.Math.Sin(rotation) * distance), (int)(origin.y + System.Math.Cos(rotation) * distance));

		public static bool operator ==(Vector2i va, Vector2i vb) =>
				va is object && vb is object &&
				 va.x == vb.x &&
				 va.y == vb.y;

		public static bool operator !=(Vector2i va, Vector2i vb) =>
				!(va == vb);

		public static Vector2i operator +(Vector2i va, Vector2i vb) =>
				new Vector2i(va.x + vb.x,
								va.y + vb.y);
		public static Vector2i operator -(Vector2i va, Vector2i vb) =>
				new Vector2i(va.x - vb.x,
								va.y - vb.y);

		public static Vector2i operator +(Vector2i v, int scalar) =>
			v + new Vector2i(scalar, scalar);
		public static Vector2i operator -(Vector2i v, int scalar) =>
			v - new Vector2i(scalar, scalar);

		public static Vector2i operator *(Vector2i va, Vector2i vb) =>
			new Vector2i(va.x * vb.x, va.y * vb.y);
		public static Vector2i operator /(Vector2i va, Vector2i vb) =>
			new Vector2i(va.x / vb.x, va.y / vb.y);

		public static Vector2i operator *(Vector2i v, int f) =>
			new Vector2i(v.x * f, v.y * f);
		public static Vector2i operator /(Vector2i v, int f) =>
			new Vector2i(v.x / f, v.y / f);
		public static Vector2i operator *(int scalar, Vector2i v) =>
			v * scalar;
		public static Vector2i operator /(int scalar, Vector2i v) =>
			v / scalar;

		public Vector2i(Vector2i v) {
			x = v.x;
			y = v.y;
		}
		public Vector2i(int x, int y) {
			this.x = x;
			this.y = y;
		}
		public Vector2i(int? x, int? y) {
			this.x = x ?? default;
			this.y = y ?? default;
		}

		public override int GetHashCode() =>
				x.GetHashCode() * 13 + y.GetHashCode() * 13;

		public override bool Equals(object obj) =>
				obj is Vector2i v && v == this;

		public static implicit operator PixelEngine.Point(Vector2i v) {
			return new PixelEngine.Point((int)v.x, (int)v.y);
		}

		public static implicit operator Vector2i((int a, int b) t) =>
			new Vector2i(t.a, t.b);

		public static int AngleBetween(Vector2i a, Vector2i b) {
			int dx = b.x - a.x;
			int dy = a.y - b.y;
			return (int)System.Math.Atan2(dy, dx);
		}

		public static implicit operator Vector2i((dynamic a, dynamic b) t) =>
			new Vector2i((int)t.a, (int)t.b);

		public static implicit operator Vector2(Vector2i vi) => (vi.x, vi.y);
		public static implicit operator Vector2i(Vector2 vf) => (vf.x, vf.y);
	}
}
