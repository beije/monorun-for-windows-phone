using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using monorun;

namespace monorun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Player player;

        //Mouse states used to track Mouse button press
        MouseState currentMouseState;
        MouseState previousMouseState;

        // A movement speed for the player
        float playerMoveSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player = new Player();
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition );
            playerMoveSpeed = 8.0f;

            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime) 
        {
            // Windows 8 Touch Gestures for MonoGame
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                

                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    player.Position += gesture.Delta;

                    System.Diagnostics.Debug.WriteLine(player.Position.X);
                   // System.Console.WriteLine(player.Position.Y.ToString());
                   // System.Console.WriteLine(player.Position.X.ToString());
                }
            }

            currentMouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 posDelta = mousePosition - player.Position;
                posDelta.Normalize();
                posDelta = posDelta * playerMoveSpeed;
                player.Position = player.Position + posDelta;

            }
            previousMouseState = currentMouseState;
            // Make sure that the player does not go out of bounds
           // player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
           // player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            _spriteBatch.Begin();

            // Draw the Player
            player.Draw(_spriteBatch);

            // Stop drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
