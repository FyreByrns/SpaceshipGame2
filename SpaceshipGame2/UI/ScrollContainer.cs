using SpaceshipGame2.Utility;
using System;
using System.Collections.Generic;

namespace SpaceshipGame2.UI {
	class ScrollContainer : UIElement {
		public List<UIElement> contents;
		public int padding = 1;
		public float scrollSpeed = 5;

		PixelEngine.Sprite backBuffer;
		TextButton scrollBar;
		float scrollLocation;
		float scrollbarHeight;
		bool scrollbarPositionChanged = false;

		/// <summary>
		/// Get the total height of all content elements, including padding
		/// </summary>
		/// <returns></returns>
		public AABB GetContentAABB() {
			AABB total = new AABB(bounds.topLeft, bounds.bottomRight);

			foreach (UIElement element in contents) {
				total = total.Combine(element.bounds);
			}

			return total;
		}

		public override void Update(SpaceGame target) {
			base.Update(target);

			int scroll = (int)target.MouseScroll;
			scrollLocation -= scroll * 5;

			scrollLocation = scrollLocation.Constrain(0, bounds.Height - scrollBar.bounds.Height);
			scrollbarPositionChanged = true;
		}

		public override void Draw(SpaceGame target) {
			base.Draw(target);
			AABB contentRect = GetContentAABB();

			int runningHeight = padding;
			runningHeight -= (int)(scrollLocation / bounds.Height * contentRect.Height);

			foreach (UIElement element in contents) {
				AABB bounds = element.bounds.AtOrigin;
				element.bounds.topLeft = topLeft + (padding, runningHeight);
				element.bounds.bottomRight = element.bounds.topLeft + bounds.bottomRight;
				runningHeight += (int)element.bounds.Height + padding;
			}

			// use back buffer for clipping
			target.DrawTarget = backBuffer;
			target.Clear(PixelEngine.Pixel.Empty);

			foreach (UIElement element in contents) {
				element.Update(target);

				if (bounds.Overlaps(element.bounds)) {
					element.Draw(target);
				}
			}

			scrollbarHeight = bounds.Height / contentRect.Height * bounds.Height;
			AABB scrollbarRect = new AABB(topRight + (-10, scrollLocation), topRight + (0, scrollbarHeight + scrollLocation));
			scrollBar.bounds = scrollbarRect;

			// only draw scrollbar if scrolling is required
			if (scrollbarHeight != size.y) {
				scrollBar.Update(target);
				scrollBar.Draw(target);
			}

			// switch back to main buffer
			PixelEngine.Sprite scrollContainerSurface = target.DrawTarget;
			target.DrawTarget = null;

			target.DrawPartialSprite(topLeft, scrollContainerSurface, topLeft, (int)bounds.Width, (int)bounds.Height);
		}

		public ScrollContainer(Vector2 position, Vector2 size) : base(position, size) {
			contents = new List<UIElement>();
			scrollLocation = 0;

			backBuffer = new PixelEngine.Sprite(SpaceGame.Instance.ScreenWidth, SpaceGame.Instance.ScreenHeight);
			scrollBar = new TextButton((0, 0), (1, 1), "");
			scrollBar.OnMouseHeld += HandleScroll;

			Random rng = new Random();
			for (int i = 0; i < 100; i++) {
				contents.Add(new TextButton((position.x, position.y /*+ (i * 10)*/), $"{i}") { currentColours = UIColourCollection.DefaultUp });
				contents[i].OnMouseClicked += (sender, mouse, button) => {
					if (sender is TextButton tb) {
						Console.WriteLine(tb.text);
					}
				};
			}
		}

		private void HandleScroll(UIElement sender, Vector2 mouse, PixelEngine.Mouse button) {
			scrollLocation = mouse.y - bounds.topLeft.y - scrollbarHeight / 2;
			scrollLocation = scrollLocation.Constrain(0, bounds.Height - scrollBar.bounds.Height);
			scrollbarPositionChanged = true;
		}
	}
}
