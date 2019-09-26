using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.IO;
using System.Net;
using System.Net.Sockets;
using PongGame.GamePong;
using PongGame.Tcp;
using PongGame.Data;


using System.Threading;
using System;
namespace PongGame.GamePong
{
    /// <summary>
    /// The score side to choose ball direction after reset
    /// </summary>
    public enum ScoreSide
    {
        GameStart = 0,
        Left = 1,
        Right = 2
    }

    /// <summary>
    /// The ball in the Pong game
    /// </summary>
    public class Ball : GameObject
    {
        #region PUBLIC FIELDS


        public static Vector2 BallPositioN = Vector2.Zero;
        /// <summary>
        /// Initial speed of the ball
        /// </summary>
        public const float SPEED = 400;

        /// <summary>
        /// The cooldown before next speed increase
        /// </summary>
        public const float SPEED_COOLDOWN = 5;

        /// <summary>
        /// The increase rate of the speed
        /// </summary>
        public const float SPEED_INCREASE_RATE = 1.1f;

        /// <summary>
        /// The direction the ball
        /// </summary>
        public Vector2 Direction;
        #endregion

        #region PRIVATE FIELDS
        private float speedIncreaseCounter = 0;
        private float speed = SPEED;

        private Vector2 startPosition;
        private Random rnd = new Random();
        #endregion

        #region CONSTRUCTERS
        public Ball(Texture2D sprite, Vector2 position) : base(sprite, position)
        {
            Sprite = sprite;
            Position = startPosition = position;
            RandomDirection();
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Gives the ball a random direction at the beginning of the game
        /// </summary>
        public void RandomDirection()
        {
            if (rnd.Next(0, 100) >= 50)
            {
                RandomDirectionRight();
            }
            else
            {
                RandomDirectionleft();
            }
        }

        /// <summary>
        /// Gives the ball a random direction towards the right side 
        /// </summary>
        public void RandomDirectionRight()
        {
            
            Direction = new Vector2((rnd.Next(50, 200)), (rnd.Next(10, 100) * 2 - 100)); 
        }

        /// <summary>
        /// Gives the ball a random direction towards the left side
        /// </summary>
        public void RandomDirectionleft() 
        {
            Direction = new Vector2((rnd.Next(0, 150) - 200), (rnd.Next(10, 100) * 2 - 100));
        }

        /// <summary>
        /// Reset the ball back to its start position and gives it back its initial speed.
        /// </summary>
        /// <param name="scoreSide">The score side used to calculate which way the ball is going next game</param>
        public void Reset(ScoreSide scoreSide)
        {
            Position = startPosition;

            speed = SPEED;

            switch (scoreSide)
            {
                case ScoreSide.Left:
                    RandomDirectionleft();
                    break;

                case ScoreSide.Right:
                    RandomDirectionRight();
                    break;

                case ScoreSide.GameStart:
                    RandomDirection();
                    break;
            }
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            if (Game1.ImClient == true && Game1.GameStart == true)
            {
                Translate((float)gameTime.ElapsedGameTime.TotalSeconds);
                Game1.GameData.BallXPosition = (int)Position.X;
                speedIncreaseCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (SPEED_COOLDOWN < speedIncreaseCounter)
                {
                    speed *= SPEED_INCREASE_RATE;
                    speedIncreaseCounter = 0;
                }

                BallPositioN = Position;
            }
            if (Game1.ImClient == false)
            {
               Position.X = GameSeverTest.ballP.X;
               Position.Y = GameSeverTest.ballP.Y;
            }
          //  Position.Y = GameSeverTest.ballP.Y;

        }
        #endregion

        #region PRIVATE FUNCTIONS
        private void Translate(float deltaTime)
        {
            Direction.Normalize();
       
                Position += Direction * speed * deltaTime;
            
        }
        #endregion
    }


}
