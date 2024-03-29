﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using monorun;
using monorun.GameClasses;

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
        Boolean collided;
        ApiHandler api;
        DateTime startGameTime;
        DateTime endGameTime;
		Boolean gameHasEnded;
		DateTime latestPlayerPositionCheck;

        public GamePage()
        {
            InitializeComponent();
            
            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            api = (Application.Current as App).api;
            
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

		/// <summary>
		/// Start all game timers and create the player object
		/// </summary>
		public void startGame()
		{
			startGameTime = DateTime.Now;
			collided = false;
			gameHasEnded = false;

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

			// TODO: use this.content to load your game content here
			//MessageBox.Show("Game start");
			// Start the timer
			timer.Start();
			AddRolands.Start();

			player = new Player();

			Vector2 playerPosition = new Vector2(20, 20);
			player.Initialize(contentManager.Load<Texture2D>("Graphics\\player"), playerPosition);

			gameItems.Add(player);

			TouchPanel.EnabledGestures = GestureType.FreeDrag;
		}

		/// <summary>
		/// Initialize the game when the user navigates to this screen
		/// </summary>
		/// <param name="e"></param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// Set the sharing mode of the graphics device to turn on XNA rendering
			SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
			gameItems = new List<GameItem>();
			rolands = new List<Roland>();
			
			latestPlayerPositionCheck = DateTime.Now.AddSeconds(5);

			Action cb = () =>
			{
				startGame();
			};

			// Start the game when the game has been
			// registered at the API
			api.getSessionId(cb);

			base.OnNavigatedTo(e);
		}

		/// <summary>
		/// Stop game timers when user move away from the game
		/// </summary>
		/// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();
            AddRolands.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

		/// <summary>
		/// Adds a new Roland to the game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        public void addRoland(object sender, GameTimerEventArgs e)
        {
            if ( collided ) return;
            int ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
            int ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;
            Random rnd = new Random();
            Vector2 enemyPosition = new Vector2(-100, -100);

            Roland enemy = new Roland();
            enemy.setPreAnimator(preAnimator );

			if( rnd.Next(0,2) == 1 ) 
			{
				enemy.Initialize(contentManager.Load<Texture2D>("Graphics\\roland_large"), enemyPosition);
			} else {
				enemy.Initialize(contentManager.Load<Texture2D>("Graphics\\roland_small"), enemyPosition);
			}

            rolands.Add( enemy );
            gameItems.Add( enemy );
        }

		/// <summary>
		/// When a collision has been detected, freeze all items for 2 seconds,
		/// post highscore and then redirect to highscore submittal page
		/// </summary>
        public void endGame() 
        {
			gameHasEnded = true;
			endGameTime = DateTime.Now;
			TimeSpan span = endGameTime - startGameTime;
			int ms = (int)span.TotalMilliseconds;
			api.LatestHighscore.score = ms;

			Action cb = () => {

				// Freeze frame for two seconds and then move on
				// to another screen.
				DispatcherTimer timeout = new DispatcherTimer();
				timeout.Interval = new TimeSpan(0, 0, 0, 0, 2000);
				timeout.Tick += (object sender, EventArgs e) =>
				{
					timer.Stop();
					NavigationService.Navigate(new Uri("/EndGamePage.xaml", UriKind.Relative));
					NavigationService.RemoveBackEntry();
					timeout.Stop(); // We only want this timer to run once, so we kill it after the first run.
				};
				timeout.Start();
			};


			api.postResult(ms, "WP - Beije", cb);
        }


        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {

			//
			// Redirect a random Roland to the player
			// if the player has stood still for longer
			// than 500ms
			//
			DateTime now = DateTime.Now;

			if (player.Position == player.lastPosition && rolands.Count > 0 )
			{
				TimeSpan sinceLastCheck = now - latestPlayerPositionCheck;
				
				if (sinceLastCheck.TotalMilliseconds > 500)
				{
					var rnd = new Random();
					var randomRoland = rolands[rnd.Next(rolands.Count)];

					// Redirect roland
					randomRoland.generateNewPosition( player.Position );
					latestPlayerPositionCheck = now;
				}
			}
			else
			{
				if (latestPlayerPositionCheck < now)
				{
					latestPlayerPositionCheck = now;
				}
			}

            //
			// Update all game items (Player and Rolands)
			//
            foreach (GameItem renderable in gameItems)
            {
                renderable.Update();
            }

			//
			// Collision detection
			//
			if( collided ) return;
            foreach( Roland roland in rolands )
            {
				if (CollisionDetection.IntersectsPixel(player.getItemRectangle(), player.getTextureData(), roland.getItemRectangle(), roland.getTextureData()))
                {
                    collided = true;
					stopAllItems();
                    break;
                }
            }
            
        }

		/// <summary>
		/// Freeze all game items (for the still-frame at end-game)
		/// </summary>
		public void stopAllItems()
		{
			foreach (GameItem renderable in gameItems)
			{
				renderable.freeze = true;
			}
		}

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {

            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(new Color(22,27,30));

            // TODO: Add your drawing code here
            // Connect rolands together
            for (int i = 0; i < rolands.Count; i = i + 2)
            {
                if (i + 1 >= rolands.Count) break;
                var basePos = rolands[i].Position;
                var newPos = rolands[i + 1].Position;

				basePos.X += (rolands[i].Width/2);
				basePos.Y += (rolands[i].Height / 2);

				newPos.X += (rolands[i + 1].Width / 2);
				newPos.Y += (rolands[i + 1].Height / 2);

                spriteBatch.Begin();
                if (basePos.Y > newPos.Y)
                {
                    DrawLine(spriteBatch, basePos, newPos);
                }
                else
                {
                    DrawLine(spriteBatch, newPos, basePos);
                }

                spriteBatch.End();
            }

			// Render out all items
			foreach (GameItem renderable in gameItems)
			{
				// Start drawing
				spriteBatch.Begin();

				// Draw the item
				renderable.Draw(spriteBatch);

				// Stop drawing
				spriteBatch.End();
			}

			if (collided && !gameHasEnded) 
            {
                endGame();
            }

        }

		/// <summary>
		/// Draws a simple line between two points
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            var t = new Texture2D(SharedGraphicsDeviceManager.Current.GraphicsDevice, 1, 1);
            t.SetData<Color>(new Color[] { Color.White });

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
				(float)Math.Atan2(edge.Y - 5, edge.X);


            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    10), //width of line, change this to make thicker line
                null,
                new Color(29, 43, 48), //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0,0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }   
}