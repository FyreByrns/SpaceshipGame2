using SpaceshipGame2.World.Graphics;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.World.Entities {
	class Player : Entity {
		public Player(vf position_w) : base(position_w) {

			graphics = MultiPolygon.FromString(@"
[	name(hull)
	comment(vertices)
		v(-0.5, -0.5) 
		v(+0.5, -0.5) 
		v(+0.2, +0.8) 
		v(-0.2, +0.8)
	colour(0, 255, 255)
	scale(20)	]
[	name(other)
	comment(vertices)
		v(-0.1, -0.1) 
		v(+0.1, -0.1) 
		v(+0.1, +0.1) 
		v(-0.1, +0.1)
	colour(100, 100, 100)
	scale(30)	]
");
		}
	}
}

