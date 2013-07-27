using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monorun
{
    class GameItem
    {
        // Animation representing the player
        public Texture2D ItemTexture;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // Get the width of the player ship
        public int Width { get { return ItemTexture.Width; } }

        // Get the height of the player ship
        public int Height { get { return ItemTexture.Height; } }

        public virtual void Initialize(Texture2D texture, Vector2 position)
        {
            ItemTexture = texture;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ItemTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
