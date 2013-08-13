using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace monorun
{
    class Player : GameItem
    {
		public Vector2 lastPosition = new Vector2();

		/// <summary>
		/// Update the player position based on touch input
		/// </summary>
        public override void Update()
        {
			if( freeze == true ) return;
			lastPosition = Position;

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    Position = gesture.Position;
					Position.X -= Width/2;
					Position.Y -= Height / 2;
                }
            }
            float ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
            float ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;

            Position.X = MathHelper.Clamp(Position.X, 0, ScreenWidth - Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, ScreenHeight - Height);
        }
    }
}
