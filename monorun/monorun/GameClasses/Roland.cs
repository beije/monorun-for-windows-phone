using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monorun
{
    class Roland : GameItem
    {
        List<Vector2> calculatedPositions;
        int speed = 20;
        int positionCounter;
        PreAnimator preAnimator;

        public Roland()
        {
            calculatedPositions = new List<Vector2>();
            speed = 10;
            positionCounter = 0;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (positionCounter >= calculatedPositions.Count)
            {
				Random rnd = new Random();

				int ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
				int ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;

				int newX = rnd.Next(0, ScreenWidth);
				int newY = rnd.Next(0, ScreenHeight);

				Vector2 endPos = new Vector2((float)newX, (float)newY);

				generateNewPosition(endPos);
            }
            if( freeze == false ) 
			{
				Position = calculatedPositions[positionCounter];
				positionCounter++;
			}

            spriteBatch.Draw(ItemTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }
        public void generateNewPosition( Vector2 newPosition )
        {
            positionCounter = 0;
			Random rnd = new Random();

            preAnimator.setStartPosition( Position );
			preAnimator.setEndPosition(newPosition);
            preAnimator.setSpeed(rnd.Next(20,100));
            calculatedPositions = new List<Vector2>();
            calculatedPositions = preAnimator.getPositions();
        }

        public void setSpeed(int spd)
        {
            speed = spd;
        }

        public void setPreAnimator(PreAnimator preanim)
        {
            preAnimator = preanim;
        }


    }
}
