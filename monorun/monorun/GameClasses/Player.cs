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
        public override void Update()
        {
			if( freeze == true ) return;

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                //System.Diagnostics.Debug.WriteLine(gesture.Delta);
                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    Position += gesture.Delta;
                }
            }
            float ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
            float ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;

            Position.X = MathHelper.Clamp(Position.X, 0, ScreenWidth - Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, ScreenHeight - Height);
        }
    }
}
