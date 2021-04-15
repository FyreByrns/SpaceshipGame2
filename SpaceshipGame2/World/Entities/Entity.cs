using PixelEngine;
using SpaceshipGame2.Utility;
using SpaceshipGame2.World.Graphics;

using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.World.Entities {
	class Entity : WorldObject, IUpdateable {
		public MultiPolygon graphics;

		#region motion
		public float rotation;
		public float rotationVelCap = 0.009f;
		public float rotationalDecayAmount = 0.02f; // hack instead of friction
		float rotationVel;
		float rotationForces;

		public vf vel;
		vf forces;
		float mass = 1;

		public void ApplyForce(vf force) => forces += force;
		public void ApplyForceAtAngle(float magnitude, float angle) {
			float cos = (float)System.Math.Cos(angle);
			float sin = (float)System.Math.Sin(angle);

			float fX = cos * magnitude;
			float fY = sin * magnitude;
			ApplyForce((fX, fY));
		}

		public void ApplyRotationalForce(float magnitude) => rotationForces += magnitude;
		#endregion motion

		public virtual void Update(SpaceGame target, float elapsed) {
			graphics.position_w = position_w;

			// a = F / m
			vel += forces / mass * elapsed;
			forces = (0, 0);

			position_w += vel;

			// rotation hack
			rotationVel += rotationForces / mass * elapsed;
			rotationForces = 0;

			if (rotationVel >= rotationVelCap || rotationVel <= -rotationVelCap)
				rotationVel = rotationVelCap * System.Math.Sign(rotationVel);
			rotation += rotationVel;
			rotationVel = rotationVel.TowardsButNotPass(0, rotationalDecayAmount * elapsed);
		}

		public override void Draw(Game target, World world) {
			graphics.rotation = rotation;
			graphics.Draw(target, world);

			//target.Draw(world.ScreenPoint(position_w), Pixel.Presets.Red);
		}

		public override AABB GetBounds() => graphics.GetBounds();

		public Entity(vf position_w) {
			this.position_w = position_w;

			graphics = MultiPolygon.FromString("[name(default)v(-0.5,-0.5)v(0.5,-0.5)v(0.5,0.5)v(-0.5,0.5)colour(255,0,0)scale(10)]");
		}
	}
}

