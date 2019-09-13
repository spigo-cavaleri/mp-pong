using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace PongGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font;
 

        public int Player1HP;
        public int Player2HP;

        public Ball ball;
        public Pad Pad1;
        public Pad Pad2;
        public Wall wallH;
        public Wall wallV;
        public Wall wallO;
        public Wall wallN;
        public  Texture2D Ball;
        public Texture2D padTextur;





        private static Game1 instance;
        public static Game1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game1();
                }
                return instance;
            }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
             ball = new Ball(Content.Load<Texture2D>("pokeBall"), new Vector2(GraphicsDevice.Viewport.Width/2,GraphicsDevice.Viewport.Height/2));
             Pad1 = new Pad(Content.Load<Texture2D>("pipe"), new Vector2(GraphicsDevice.Viewport.Width / 12, GraphicsDevice.Viewport.Height / 2),"pad1");
             Pad2 = new Pad(Content.Load<Texture2D>("pipe"), new Vector2(GraphicsDevice.Viewport.Width /1.1f, GraphicsDevice.Viewport.Height / 2),"pad2");

             wallH = new Wall(Content.Load<Texture2D>("nada"), new Vector2(GraphicsDevice.Viewport.Width ,0 ), 1, GraphicsDevice.Viewport.Height,"WallRight");//right
             wallV = new Wall(Content.Load<Texture2D>("nada"), new Vector2(0, 0), 1, GraphicsDevice.Viewport.Height, "WallLeft");//left

            wallO = new Wall(Content.Load<Texture2D>("nada"), new Vector2(0,0), GraphicsDevice.Viewport.Width, 1,"WallNed");//button
            wallN = new Wall(Content.Load<Texture2D>("nada"), new Vector2(0,GraphicsDevice.Viewport.Height), GraphicsDevice.Viewport.Width,1, "WallUp");//top


             Font = Content.Load<SpriteFont>("Text");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ball.Update(gameTime);
            Pad1.Update(gameTime);
            Pad2.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            ball.Draw(spriteBatch);
            Pad1.Draw(spriteBatch);
            Pad2.Draw(spriteBatch);
            wallH.Draw(spriteBatch);
            wallV.Draw(spriteBatch);
            wallO.Draw(spriteBatch);
            wallN.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
