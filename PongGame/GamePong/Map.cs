﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PongGame.Tcp;
using System;
using System.Collections.Generic;

namespace PongGame.GamePong
{
    public class Map
    {
        #region CONST VALUES
        /// <summary>
        /// The pad off set
        /// </summary>
        public const int PadOffSetFromEgde = 50;
        #endregion

        public bool IsServer
        {
            get => this.isServer;
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
                // Receive
                Data.TcpDataPacket[] data = gameClient.GetDataToReceive();
                for (int i = 0; i < data.Length; i++)
                {
                    string received = data[i].Data;
                    string[] split = received.Split(':');
                    float receivedOtherY = (int)Convert.ToInt32(split[0]);
                    float receivedMeY = (int)Convert.ToInt32(split[1]);
                    float receivedBallX = (int)Convert.ToInt32(split[2]);
                    float receivedBallY = (int)Convert.ToInt32(split[3]);

                    player1Pad.Position = new Vector2(player1Pad.Position.X, receivedOtherY);
                    player2Pad.Position = new Vector2(player2Pad.Position.X, receivedMeY);
                    ball.Position = new Vector2(receivedBallX, receivedBallY);
                }

                // Game Logic
                MPKeyPress intent = player2Pad.ClientIntent();

                // Send
                string min = "intent:" + (int)intent;
                gameClient.SetDataToSend(min);
            }
            else
            {
                // Receive
                Data.TcpDataPacket[] data = GameServer.Instance.GetDataToReceive();
                for (int i = 0; i < data.Length; i++)
                {
                    string received = data[i].Data;
                    string[] split = received.Split(':');
                    MPKeyPress intent = (MPKeyPress) Convert.ToInt32(split[1]);

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
                string min = Math.Round(player1Pad.Position.Y) + ":" + Math.Round(player2Pad.Position.Y) + ":" + Math.Round(ball.Position.X) + ":" + Math.Round(ball.Position.Y);
                GameServer.Instance.BroadCast(min);
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
        #endregion
    }
}
