using System.Collections.Generic;

using Microsoft.Xna.Framework;
using PongGame.Network;

namespace PongGame.MPPongGame
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


                float padMidtPunkt = pad.Position.Y + pad.Height / 2;
                float ballmidpount = ball.Position.Y + ball.Height / 2;

                float DistanceFromMidt = padMidtPunkt - ballmidpount;


                if (ballmidpount > padMidtPunkt)
                {
                    ball.Direction.Y += -DistanceFromMidt / 130;
                }
                if (ballmidpount < padMidtPunkt)
                {
                    ball.Direction.Y -= DistanceFromMidt / 130;
                }

                ball.Direction.X *= INVERT;

                // Wonky collision fix
                if (ball.HitBox.X > pad.HitBox.X)
                {
                    ball.Position.X += ball.HitBox.X - pad.HitBox.X;
                }
                else
                {
                    ball.Position.X += ball.HitBox.X - (pad.HitBox.X - pad.HitBox.Width);
                }
                ball.PointUpdater();
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

            Map.Instance.Player1Pad.DeductHealth();
            Map.Instance.Player2Pad.AddPoints(Map.Instance.Ball.PointCounter);

            if (Map.Instance.Player1Pad.HealthPoints <= 0)
            {
                // Do nothing cause I (Server player) lost the game :(
                Map.Instance.GameOver = true;
            }
            
            ball.Reset(ScoreSide.Left);
            ball.PointResetter();
        }

        private void BallCollideWithRightWall(Ball ball)
        {
            Map.Instance.Player2Pad.DeductHealth();
            Map.Instance.Player1Pad.AddPoints(Map.Instance.Ball.PointCounter);

            if (Map.Instance.Player2Pad.HealthPoints <= 0)
            {
                // I (Server player) won and will submit highscore!
                RequestHTTP.SendHighscore(Map.Instance.MyUsername, Map.Instance.Player1Pad.CurrentPoints);
                Map.Instance.GameOver = true;
            }
            
            ball.Reset(ScoreSide.Right);
            ball.PointResetter();
        }
        #endregion
    }
}
