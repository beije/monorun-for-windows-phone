using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using monorun;

namespace monorun
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        Player player;
        List<GameItem> gameItems;
        List<Roland> rolands;
        GameTimer AddRolands;
        PreAnimator preAnimator;

        //Mouse states used to track Mouse button press
        MouseState currentMouseState;
        MouseState previousMouseState;

        // A movement speed for the player
        float playerMoveSpeed;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            preAnimator = new PreAnimator();

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(166667); // 60fps
            //timer.UpdateInterval = TimeSpan.FromTicks(333333); 30fps
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            AddRolands = new GameTimer();
            AddRolands.UpdateInterval = TimeSpan.FromTicks(10000*2000);
            AddRolands.Update += addRoland;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
            gameItems = new List<GameItem>();
            rolands = new List<Roland>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here
            //MessageBox.Show("Game start");
            // Start the timer
            timer.Start();
            AddRolands.Start();

            player = new Player();
            playerMoveSpeed = 8.0f;
            Vector2 playerPosition = new Vector2(20, 20);
            player.Initialize(contentManager.Load<Texture2D>("Graphics\\player"), playerPosition);

            gameItems.Add( player );

            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();
            AddRolands.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        public void addRoland(object sender, GameTimerEventArgs e)
        {
            int ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
            int ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;
            Random rnd = new Random();
            Vector2 enemyPosition = new Vector2(-100, -100);

            Roland enemy = new Roland();
            enemy.setPreAnimator(preAnimator );
            enemy.Initialize(contentManager.Load<Texture2D>("Graphics\\roland_large"), enemyPosition);

            rolands.Add( enemy );
            gameItems.Add( enemy );
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {

            // TODO: Add your update logic here
            foreach (GameItem renderable in gameItems)
            {
                renderable.Update();
            }
            
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            foreach ( GameItem renderable in gameItems)
            {
                // Start drawing
                spriteBatch.Begin();

                // Draw the item
                renderable.Draw(spriteBatch);

                // Stop drawing
                spriteBatch.End();
            }
            

        }
    }
}