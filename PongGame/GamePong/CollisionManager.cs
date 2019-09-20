using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace PongGame.GamePong
{
    public class CollisionManager
    {
        #region CONST VALUES
        /// <summary>
        /// Invert
        /// </summary>
        public const int INVERT = -1;
        #endregion

        #region EVENTS
        /// <summary>
        /// Event for when players has scored
        /// </summary>
        public event ScoreHandle HasScored;
        public delegate void ScoreHandle(int scoreSubstraction, string player);
        #endregion

        #region SINGLETON
        /// <summary>
        /// The instance of the collision manager
        /// </summary>
        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionManager();
                }

                return instance;
            }
        }

        private static CollisionManager instance;
        #endregion

        #region PRIVATE FIELDS
        private Map map;
        private List<GameObject> gameObjects;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a collision manager
        /// </summary>
        public CollisionManager()
        {
            map = Map.Instance;
            gameObjects = map.GameObjects;
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(!Map.Instance.IsServer)
            {
                return;
            }
            Collision();
        }
        #endregion

        #region PRIVATE FUNCTIONS
        private void Collision()
        {
            GameObject[] colliderObjects = gameObjects.ToArray();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = 0; j < colliderObjects.Length; j++)
                {
                    if (colliderObjects[j] is Wall && gameObjects[i] is Ball)
                    {
                        BallCollideWithWall(gameObjects[i] as Ball, colliderObjects[j] as Wall);
                    }

                    if (colliderObjects[j] is Pad && gameObjects[i] is Ball)
                    {
                        BallCollideWithPad(gameObjects[i] as Ball, colliderObjects[j] as Pad);
                    }
                }
            }
        }

        private void BallCollideWithPad(Ball ball, Pad pad)
        {
            if (ball.IsColliding(pad))
            {
                ball.Direction.X *= INVERT;
            }
        }

        private void BallCollideWithWall(Ball ball, Wall wall)
        {
            if (ball.IsColliding(wall))
            {
                string wallName = wall.Name;

                switch (wallName)
                {
                    case "TopWall":
                        BallCollideWithTopAndButtomWall(ball);
                        break;

                    case "ButtomWall":
                        BallCollideWithTopAndButtomWall(ball);
                        break;

                    case "LeftWall":
                        BallCollideWithLeftWall(ball);
                        break;

                    case "RightWall":
                        BallCollideWithRightWall(ball);
                        break;
                }
            }
        }

        private void BallCollideWithTopAndButtomWall(Ball ball)
        {
            ball.Direction.Y *= INVERT;
        }

        private void BallCollideWithLeftWall(Ball ball)
        {
            if (HasScored != null)
            {
                HasScored(-1, "player1");
            }

            ball.Reset(ScoreSide.Left);
        }

        private void BallCollideWithRightWall(Ball ball)
        {
            if (HasScored != null)
            {
                HasScored(-1, "player2");
            }

            ball.Reset(ScoreSide.Right);
        }
        #endregion
    }
}
