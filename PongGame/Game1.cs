using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using PongGame.GamePong;
using PongGame.Tcp;
using PongGame.Data;

namespace PongGame
{
    public enum GameState { LoginScreen, WaitingForPlayer, Playing }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region SINGLETON

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

        private static Game1 instance;
        #endregion

        public GraphicsDeviceManager GraphicsDeviceManager;
        
        public SpriteBatch SpriteBatch
        {
            get;
        }
        public GameState GameState {
            get => gameState;
            set
            {
                gameState = value;
                if (value == GameState.LoginScreen)
                {
                    this.IsMouseVisible = true;
                }
                else
                {
                    this.IsMouseVisible = false;
                }
            }
        }

        private GameState gameState;
        private Map pongMap;
        private SpriteFont font;
        public static GameData GameData;
        private GameClientTest clientTest = new GameClientTest();
        private TcpDataPacket TcpData = new TcpDataPacket();


        public Game1()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            GraphicsDeviceManager.PreferredBackBufferWidth = 1024;  // set this value to the desired width of your window
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            GraphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GameState = GameState.Playing;
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
            pongMap = Map.Instance;
            GameData = new GameData();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            pongMap.LoadContent();
            LoginScreen.Instance.LoadContent();
            LobbyScreen.Instance.LoadContent();

            font = Content.Load<SpriteFont>("Font");
            clientTest.Connect("127.0.0.1", "lluliululalala");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

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


           

            
            switch (GameState)
            {
                case GameState.LoginScreen:
                    LoginScreen.Instance.Update(gameTime);
                    break;
                case GameState.WaitingForPlayer:
                    LobbyScreen.Instance.Update(gameTime);
                    break;
                case GameState.Playing:
                    pongMap.Update(gameTime);
                    break;
                default:
                    break;
            }

          


            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(font, $"Player 1 Name        {GameData.BallXPosition}             player 2 Name", new Vector2(GraphicsDevice.Viewport.Width/2-100, 20), Color.Red);


            switch (GameState)
            {
                case GameState.LoginScreen:
                    LoginScreen.Instance.Draw(SpriteBatch);
                    break;
                case GameState.WaitingForPlayer:
                    LobbyScreen.Instance.Draw(SpriteBatch);
                    break;
                case GameState.Playing:
                    pongMap.Draw(SpriteBatch);
                    break;
                default:
                    break;
            }

            SpriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


    }
}
