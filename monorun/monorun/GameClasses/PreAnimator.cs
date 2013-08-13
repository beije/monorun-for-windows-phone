using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace monorun
{
    class PreAnimator
    {
        List<Vector2> positions;
        Vector2 startPosition = new Vector2(0, 0);
        Vector2 endPosition = new Vector2(0, 0);
        int speed = 0;
        Dictionary<string, int> calculatedPosition;

        public PreAnimator()
        {
            positions = new List<Vector2>();

            calculatedPosition = new Dictionary<string, int>();
            calculatedPosition.Add("startX",0);
            calculatedPosition.Add("startY",0);
            calculatedPosition.Add("endX",0);
            calculatedPosition.Add("endY",0);

        }

		/// <summary>
		/// Calculates every step in the animation.
		/// The steps are saved to Positions
		/// </summary>
        private void calculateAnimation()
        {
            positions = new List<Vector2>();
            bool xPos = false;
            bool yPos = false;
            int counter = 0;

            float diffX = (endPosition.X > startPosition.X ? endPosition.X - startPosition.X : startPosition.X - endPosition.X);
            float diffY = (endPosition.Y > startPosition.Y ? endPosition.Y - startPosition.Y : startPosition.Y - endPosition.Y);

            Vector2 lastCorrectPosition = startPosition;
            

            while (!yPos || !xPos)
            {
                double modifier = (double)counter / (double)speed;
                float x = (float)(Math.Sin(modifier) * (double)diffX);
                float y = (float)(Math.Sin(modifier) * (double)diffY);

                float latestCorrectX = (xPos == false ? x : diffX);
                float latestCorrectY = (yPos == false ? y : diffY);

                float nextPositionX = (endPosition.X > startPosition.X ? startPosition.X + latestCorrectX : startPosition.X - latestCorrectX);
                float nextPositionY = (endPosition.Y > startPosition.Y ? startPosition.Y + latestCorrectY : startPosition.Y - latestCorrectY);

                positions.Add(new Vector2(nextPositionX, nextPositionY));

                if ((int)x >= (int)diffX - 1)
                {
                    xPos = true;
                }
                if ((int)y >= (int)diffY - 1)
                {
                    yPos = true;
                }

                counter++;

                if (counter > 500)
                {
                    yPos = true;
                    xPos = true;
                    break;
                }
            }

            // Push in as the last step to complete the animation
            positions.Add(new Vector2(endPosition.X, endPosition.Y));
        }

		/// <summary>
		/// Set start position of animation
		/// </summary>
		/// <param name="position"></param>
        public void setStartPosition(Vector2 position)
        {
            startPosition = position;
            calculatedPosition["startX"] = (int) position.X;
            calculatedPosition["startY"] = (int) position.Y;
        }

		/// <summary>
		/// Set end position of animation
		/// </summary>
		/// <param name="position"></param>
        public void setEndPosition(Vector2 position)
        {
            endPosition = position;
            calculatedPosition["endX"] = (int)position.X;
            calculatedPosition["endY"] = (int)position.Y;
        }

		/// <summary>
		/// Set speed of animation
		/// </summary>
		/// <param name="sp">the animation speed, lower == faster</param>
        public void setSpeed(int sp)
        {
            speed = sp;    
        }

		/// <summary>
		/// Returns the prerendered animation
		/// </summary>
		/// <returns>A list with all the positions</returns>
        public List<Vector2> getPositions()
        {
            calculateAnimation();
            return positions;
        }
    }
}
