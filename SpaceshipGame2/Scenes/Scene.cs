namespace SpaceshipGame2.Scenes {
	abstract class Scene {
		public SpaceGame owner;
		public abstract void Update(float elapsed);

		public Scene(SpaceGame owner) {
			this.owner = owner;
		}
	}
}

