using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monorun
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;

        // Get the width of the player ship
        public int Width
        {

            get { return PlayerTexture.Width; }

        }

        // Get the height of the player ship
        public int Height
        {

            get { return PlayerTexture.Height; }

        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlayerTexture = texture;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
