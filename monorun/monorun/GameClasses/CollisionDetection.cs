using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monorun.GameClasses
{
	static class CollisionDetection
	{
		/// <summary>
		/// Checks if two items collided (based on rectangle data)
		/// (fast but inaccurate)
		/// </summary>
		/// <param name="source">The first game item</param>
		/// <param name="target">The second game item</param>
		/// <returns></returns>
		static public bool intersectsBox(Rectangle source, Rectangle target)
		{
			return !(
				((source.Y + source.Height) < (target.Y)) ||
				(source.Y > (target.Y + target.Height)) ||
				((source.X + source.Width) < target.X) ||
				(source.X > (target.X + target.Width))
			);

		}

		/// <summary>
		/// Checks if two items collide on the pixel level
		/// (Slow but accurate)
		/// </summary>
		/// <param name="rect1">The first item's position and size as rectangle</param>
		/// <param name="data1">The first item's texturedata</param>
		/// <param name="rect2">The second item's position and size as rectangle</param>
		/// <param name="data2">The second item's texturedata</param>
		/// <returns></returns>
		static public bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
		{
			if (!intersectsBox(rect1, rect2)) return false;

			int top = Math.Max(rect1.Top, rect2.Top);
			int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
			int left = Math.Max(rect1.Left, rect2.Left);
			int right = Math.Min(rect1.Right, rect2.Right);

			for (int y = top; y < bottom; y++)
			{
				for (int x = left; x < right; x++)
				{
					Color colour1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
					Color colour2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

					if (colour1.A != 0 && colour2.A != 0)
					{
						return true;
					}

				}
			}

			return false;
		}
	}
}
