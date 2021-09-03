using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PixelEngine;
using SpaceshipGame2.Utility;
using SpaceshipGame2.World.Entities;
using vf = SpaceshipGame2.Utility.Vector2;

namespace SpaceshipGame2.UI {
	class HUD : UIElement {
		Player player;

		public override void Update(SpaceGame target) {
			bounds.bottomRight = (target.ScreenWidth, target.ScreenHeight);
		}

		public override void Draw(SpaceGame target) {
			target.PixelMode = Pixel.Mode.Alpha;

			// motion information
			// box
			vf boxTopLeft = bounds.bottomRight - 60;
			vf boxBottomRight = boxTopLeft + 50;
			vf boxMiddle = boxTopLeft + 25;
			target.FillRect(boxTopLeft, boxBottomRight, new Pixel(200, 100, 100, 100));

			vf dirOfVel = player.vel.Normalized() * Math.Min(player.vel.Length * 10, 23);
			vf dirOfShip = vf.Along(default, 10, -player.rotation + (float)Math.PI / 2);

			target.DrawText(boxTopLeft, $"h", Pixel.Presets.Beige);
			target.DrawText(boxTopLeft + (9, 0), "v", Pixel.Presets.Red);

			target.DrawLine(boxMiddle, boxMiddle + dirOfShip, Pixel.Presets.Beige);
			target.DrawLine(boxMiddle, boxMiddle + dirOfVel, Pixel.Presets.Red);

			target.PixelMode = Pixel.Mode.Normal;
		}

		public HUD(Player player) : base(default, default) {
			this.player = player;
		}
	}
}
