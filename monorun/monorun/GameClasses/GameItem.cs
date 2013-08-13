using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace monorun
{
    class GameItem
    {
        public Texture2D ItemTexture;
        private Color[] textureData;

		// If the object should be frozen
		public Boolean freeze { get; set; }
       
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // Get the width of the item
        public int Width { get { return ItemTexture.Width; } }

        // Get the height of the item
        public int Height { get { return ItemTexture.Height; } }

		/// <summary>
		/// Initializes the game item
		/// </summary>
		/// <param name="texture">The item's texture</param>
		/// <param name="position">The start position</param>
        public virtual void Initialize(Texture2D texture, Vector2 position)
        {
			freeze = false;
            ItemTexture = texture;
            textureData = generateTextureData();
            Position = position;
        }

        public virtual void Update() {}

		/// <summary>
		/// Draws out the item at the correct position
		/// </summary>
		/// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ItemTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

		/// <summary>
		/// Generates a color array based on the item texture
		/// </summary>
		/// <returns></returns>
        private Color[] generateTextureData()
        {
            Color[] td = new Color[ItemTexture.Width * ItemTexture.Height];
            ItemTexture.GetData(td);

            return td;
        }

		/// <summary>
		/// Returns the texture data
		/// </summary>
		/// <returns></returns>
        public virtual Color[] getTextureData() {
            return textureData;
        }

		/// <summary>
		/// Gets a Rectangle based on the size and position of the item
		/// used for simple hit tests
		/// </summary>
		/// <returns></returns>
        public virtual Rectangle getItemRectangle() 
        {
            Rectangle rect = new Rectangle( (int)(Position.X-(ItemTexture.Width/2)), (int)(Position.Y-(ItemTexture.Height/2)), ItemTexture.Width, ItemTexture.Height );

            return rect;
        }
    }
}
