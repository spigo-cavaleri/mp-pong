using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace PongGame.GamePong
{
    public class CollisionManager
    {
        public const int INVERT = -1;

        public event ScoreHandle HasScored;
        public delegate void ScoreHandle(int scoreSubstraction, string player);

        #region SINGLETON
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

        private Map map;
        private List<GameObject> gameObjects;

        public CollisionManager()
        {
            map = Map.Instance;
            gameObjects = map.GameObjects;
        }

        public void Update(GameTime gameTime)
        {
            Collision();
        }

        private void Collision()
        {
            GameObject[] colliderObjects = gameObjects.ToArray();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (!(gameObjects[i] is Ball))
                {
                    continue;
                }

                for (int j = 0; j < colliderObjects.Length; j++)
                {
                    if (colliderObjects[i] is Wall)
                    {
                        BallCollideWithWall(gameObjects[i] as Ball, colliderObjects[i] as Wall);
                    }
                    else if (gameObjects[i] is Pad)
                    {
                        BallCollideWithPad(colliderObjects[i] as Ball);
                    }
                }
            }
        }

        private void BallCollideWithPad(Ball ball)
        {
            ball.Direction.X *= INVERT;
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
    }
}
