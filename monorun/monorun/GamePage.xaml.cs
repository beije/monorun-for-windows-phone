using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
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
        Boolean collided;
        ApiHandler api;

        public GamePage()
        {
            InitializeComponent();
            api = new ApiHandler();
            System.Diagnostics.Debug.WriteLine( api.isOnline );
            api.doRequest("get");
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
            api.doRequest("register");
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
            //if (rolands.Count >= 5) return;
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
            collided = false;
            foreach( Roland roland in rolands )
            {
                if (IntersectsPixel(player.getItemRectangle(), player.getTextureData(), roland.getItemRectangle(), roland.getTextureData()))
                {
                    collided = true;
                    break;
                }
            }
            
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            if (!collided)
            {
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(new Color(22,27,30));
            }
            else
            {
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Red);     
            }
            



            // TODO: Add your drawing code here

            // Connect rolands together
            for (int i = 0; i < rolands.Count; i = i + 2)
            {
                if (i + 1 >= rolands.Count) break;
                var basePos = rolands[i].Position;
                var newPos = rolands[i + 1].Position;

                basePos.X += 31;
                basePos.Y += 36;

                newPos.X += 31;
                newPos.Y += 36;

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

        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            var t = new Texture2D(SharedGraphicsDeviceManager.Current.GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


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

        static bool intersectsBox( Rectangle source, Rectangle target ) {
		    return !(
                ((source.Y + source.Height) < (target.Y)) ||
			    ( source.Y > ( target.Y + target.Height ) ) ||
			    ( ( source.X + source.Width ) < target.X ) ||
                (source.X > (target.X + target.Width)) 
		    );

        }

        static bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            if (!intersectsBox(rect1, rect2)) return false;

            int top = Math.Max( rect1.Top, rect2.Top );
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colour1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color colour2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (colour1.A != 0 && colour2.A != 0)
                    {
                        return true;
                    }
                    
                }
            }

            return false;
        }
    }   
}