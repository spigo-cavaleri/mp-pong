using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PongGame.Network;
using PongGame.Network.Tcp;
using PongGame.Network.Tcp.Data;

namespace PongGame.MPPongGame
{
    public class Map
    {
        #region CONST VALUES
        /// <summary>
        /// The pad off set
        /// </summary>
        public const int PadOffSetFromEgde = 50;
        #endregion

        public SpriteFont GameFont
        {
            get => this.gameFont;
        }

        public bool IsServer
        {
            get => this.isServer;
        }

        public Pad Player1Pad
        {
            get => this.player1Pad;
        }

        public Pad Player2Pad
        {
            get => this.player2Pad;
        }

        public Ball Ball
        {
            get => this.ball;
        }

        #region SINGLETON
        /// <summary>
        /// The instance of the map
        /// </summary>
        public static Map Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Map();
                }

                return instance;
            }
        }

        private static Map instance;
        #endregion

        #region PUBLIC FIELDS
        /// <summary>
        /// True if the game has been won, false otherwise
        /// </summary>
        public bool WonGame = false;

        /// <summary>
        /// Player one name
        /// </summary>
        public string PlayerOneName = null;

        /// <summary>
        /// Player two name
        /// </summary>
        public string PlayerTwoName = null;

        public string MyUsername = "";

        public bool GameOver = false;

        /// <summary>
        /// List of active game objects on the map
        /// </summary>
        public List<GameObject> GameObjects = new List<GameObject>();
        #endregion

        #region PRIVATE FIELDS
        private Texture2D backgroundSprite;
        private Texture2D ballSprite;
        private Texture2D player1PadSprite;
        private Texture2D player2PadSprite;

        private SpriteFont gameFont;

        private Wall topWall;
        private Wall buttomWall;
        private Wall leftWall;
        private Wall rightWall;

        private Pad player1Pad;
        private Pad player2Pad;

        private Ball ball;

        private CollisionManager collisionManager;

        private ContentManager content;
        private GraphicsDevice graphics;

        private GameClient gameClient;
        private bool isServer = false;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constucts a map where the players can play Pong
        /// </summary>
        private Map()
        {
            content = Game1.Instance.Content;
            graphics = Game1.Instance.GraphicsDevice;

            InitColliderWalls();
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Loads the content for the game
        /// </summary>
        public void LoadContent()
        {
            ballSprite = content.Load<Texture2D>("pokeBall");
            player1PadSprite = content.Load<Texture2D>("pipe");
            player2PadSprite = content.Load<Texture2D>("pipe");

            gameFont = content.Load<SpriteFont>("InputFont");

            collisionManager = CollisionManager.Instance;

            InitBall();
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(!this.isServer)
            {
                HandleClientUpdate(gameTime);
            }
            else
            {
                HandleServerUpdate(gameTime);
            }
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundSprite != null)
            {
                spriteBatch.Draw(backgroundSprite, new Vector2(), Color.White);
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(spriteBatch);
            }
        }

        public void SetupAsServer()
        {
            // HACKY WACKY SOLUTION HVIS IKKE VI DOTTER IND PÅ EXCEPTION :(
            // GameServer.Instance.GetType();

            GameServer.Instance.GameServerException += Instance_GameServerException;

            this.isServer = true;
            InitPlayers();
        }

        public void SetupAsClient(string ip, string port)
        {
            this.isServer = false;
            this.gameClient = new GameClient(ip, (ushort)Convert.ToInt32(port));

            Game1.Instance.GameState = GameState.Playing;

            gameClient.GameClientException += GameClient_GameClientException;

            InitPlayers();
        }

        private void GameClient_GameClientException(string exceptionMessage)
        {
            throw new Exception(exceptionMessage);
        }

        private void Instance_GameServerException(string exceptionMessage)
        {
            throw new Exception(exceptionMessage);
        }

        #endregion

        #region PRIVATE FUNCTIONS
        private void InitColliderWalls()
        {
            Vector2 topWallPosition = new Vector2();

            topWall = new Wall(null, topWallPosition, "TopWall");
            topWall.SetHeight(1);
            topWall.SetWidth((int)graphics.Viewport.Width);

            Vector2 buttomWallPosition = new Vector2(0, graphics.Viewport.Height - 1);

            buttomWall = new Wall(null, buttomWallPosition, "ButtomWall");
            buttomWall.SetHeight(1);
            buttomWall.SetWidth((int)graphics.Viewport.Width);

            Vector2 leftWallPosition = new Vector2(-2, 0);

            leftWall = new Wall(null, leftWallPosition, "LeftWall");
            leftWall.SetHeight((int)graphics.Viewport.Height);
            leftWall.SetWidth(5);

            Vector2 rightWallPosition = new Vector2(graphics.Viewport.Width - 3, 0);

            rightWall = new Wall(null, rightWallPosition, "RightWall");
            rightWall.SetHeight((int)graphics.Viewport.Height);
            rightWall.SetWidth(5);

            GameObjects.Add(topWall);
            GameObjects.Add(buttomWall);
            GameObjects.Add(leftWall);
            GameObjects.Add(rightWall);
        }

        private void InitPlayers()
        {
            // player 1 start position
            int p1Width = PadOffSetFromEgde;
            int p1Heigth = (graphics.Viewport.Height / 2) - (player1PadSprite.Height / 2);
            Vector2 startPositionPlayerOne = new Vector2(p1Width, p1Heigth);
            player1Pad = new Pad(player1PadSprite, startPositionPlayerOne, PlayerOneName, this.isServer);

            int p2Width = graphics.Viewport.Width - player2PadSprite.Width - PadOffSetFromEgde;
            int p2Height = (graphics.Viewport.Height / 2) - (player1PadSprite.Height / 2);
            Vector2 startPositionPlayerTwo = new Vector2(p2Width, p2Height);
            player2Pad = new Pad(player2PadSprite, startPositionPlayerTwo, PlayerTwoName, !this.isServer);

            GameObjects.Add(player1Pad);
            GameObjects.Add(player2Pad);
        }

        private void InitBall()
        {
            float middlePointHeight = (graphics.Viewport.Height / 2) - (ballSprite.Height / 2);
            float middlePointWidth = (graphics.Viewport.Width / 2) - (ballSprite.Width / 2);

            Vector2 startPosition = new Vector2(middlePointWidth, middlePointHeight);
            ball = new Ball(ballSprite, startPosition);

            GameObjects.Add(ball);
        }

        private void HandleServerUpdate(GameTime gameTime)
        {
            // Receive
            ServerUpdateDataPacket[] serverUpdateDPs = GameServer.Instance.GetDataToReceive<ServerUpdateDataPacket>();
            for (int i = 0; i < serverUpdateDPs.Length; i++)
            {
                MPKeyPress intent = serverUpdateDPs[i].MPKeyPress;

                player2Pad.HandleClientIntent(gameTime, intent);
            }

            // Game Logic
            if (GameObjects != null)
            {
                for (int i = 0; i < GameObjects.Count; i++)
                {
                    GameObjects[i].Update(gameTime);
                }

                collisionManager.Update(gameTime);
            }

            // Send
            int playerMeY = (int)Math.Round(player1Pad.Position.Y);
            int playerOtherY = (int)Math.Round(player2Pad.Position.Y);
            int ballX = (int)Math.Round(ball.Position.X);
            int ballY = (int)Math.Round(ball.Position.Y);

            ClientUpdateDataPacket clientUpdateDP = new ClientUpdateDataPacket(playerMeY, playerOtherY, ballX, ballY, Player1Pad.CurrentPoints, Player1Pad.HealthPoints, Player2Pad.CurrentPoints, Player2Pad.HealthPoints);
            GameServer.Instance.BroadCast(clientUpdateDP);
        }

        private void HandleClientUpdate(GameTime gameTime)
        {
            // Receive
            ClientUpdateDataPacket[] cUpdateDPs = gameClient.GetDataToReceive<ClientUpdateDataPacket>();

            //for (int i = 0; i < data.Length; i++)
            //{
            // Tager kun sidste modtagne pakke :)
            if (cUpdateDPs.Length > 0)
            {
                ClientUpdateDataPacket cUPD = cUpdateDPs[cUpdateDPs.Length - 1];

                player1Pad.Position = new Vector2(player1Pad.Position.X, cUPD.SPPositionY);
                player2Pad.Position = new Vector2(player2Pad.Position.X, cUPD.CPPositionY);
                ball.Position = new Vector2(cUPD.BallPositionX, cUPD.BallPositionY);

                player1Pad.ClientUpdateStatsFromServer(cUPD.SPoints, cUPD.SHealth);
                player2Pad.ClientUpdateStatsFromServer(cUPD.CPoints, cUPD.CHealth);

                if(player1Pad.HealthPoints <= 0)
                {
                    // Other player (the server player) lost! I won yay
                    RequestHTTP.SendHighscore(MyUsername, player2Pad.CurrentPoints);
                }
            }

            // Game Logic
            MPKeyPress intent = player2Pad.ClientIntent();

            // Send
            ServerUpdateDataPacket sUDP = new ServerUpdateDataPacket(intent);
            gameClient.SetDataToSend(sUDP);
        }
        #endregion
    }
}
