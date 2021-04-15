using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.World.Entities {
	class Player : Entity {
		public Player(vf position_w) : base(position_w) {

			graphics = IO.AssetManager.GetShape("player");
		}
	}
}

