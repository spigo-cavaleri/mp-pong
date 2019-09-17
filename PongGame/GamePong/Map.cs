﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constucts a map where the players can play Pong
        /// </summary>
        public Map()
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

            InitPlayers();
            InitBall();

            PlayerOneName = "Niki";
            PlayerTwoName = "NotNiki";
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (GameObjects != null)
            {
                if (!(string.IsNullOrEmpty(PlayerOneName) && string.IsNullOrEmpty(PlayerTwoName)))
                {
                    for (int i = 0; i < GameObjects.Count; i++)
                    {
                        GameObjects[i].Update(gameTime);
                    }
                }

                collisionManager.Update(gameTime);
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
            player1Pad = new Pad(player1PadSprite, startPositionPlayerOne, PlayerOneName);

            int p2Width = graphics.Viewport.Width - player2PadSprite.Width - PadOffSetFromEgde;
            int p2Height = (graphics.Viewport.Height / 2) - (player1PadSprite.Height / 2);
            Vector2 startPositionPlayerTwo = new Vector2(p2Width, p2Height);
            player2Pad = new Pad(player2PadSprite, startPositionPlayerTwo, PlayerTwoName);

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