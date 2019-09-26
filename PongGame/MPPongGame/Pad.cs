using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame.MPPongGame
{
    /// <summary>
    /// Multiplayer key press to control the pad
    /// </summary>
    public enum MPKeyPress
    {
        None = 0,
        Up = 1,
        Down = 2
    }

    public class Pad : GameObject
    {
        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X + 16, (int)Position.Y + 20, Width - 32, Height - 40);
            }
        }

        #region PUBLIC PROPERTIES

        public int HealthPoints
        {
            get => this.healthPoints;
        }

        public int CurrentPoints
        {
            get => this.currentPoints;
        }

        /// <summary>
        /// The mulitplayer key press to send over the network
        /// </summary>
        public MPKeyPress keyPress
        {
            get;
            set;
        } = MPKeyPress.None;

        /// <summary>
        /// The move speed of the pad
        /// </summary>
        public float Speed
        {
            get;
            set;
        } = 400f;
        #endregion

        #region PUBLIC FIELDS
        /// <summary>
        /// Name of the pad
        /// </summary>
        public string Name;
        #endregion

        private bool controllable;

        private int healthPoints;
        private int currentPoints;

        #region CONSTRUCTERS
        /// <summary>
        /// Construct a pad
        /// </summary>
        /// <param name="sprite">The sprite used for the pad</param>
        /// <param name="position">The position of the pad</param>
        /// <param name="name">the name of the pad</param>
        public Pad(Texture2D sprite, Vector2 position, string name, bool controllable) : base(sprite, position)
        {
            Name = name;
            Sprite = sprite;
            Position = position;
            this.controllable = controllable;
            this.healthPoints = 5;
            this.currentPoints = 0;
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            if (this.controllable)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && HitBox.Y > 0)
                {
                    Position.Y -= (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && HitBox.Y < Game1.Instance.GraphicsDevice.Viewport.Height - HitBox.Height)
                {
                    Position.Y += (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            else
            {
                Translate((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Hacky wacky hardcoded position for text display (<_<)
            Vector2 healthTextPosition = new Vector2((Position.X < 300) ? Position.X : Position.X - 70, 20);
            Vector2 pointsTextPosition = new Vector2((Position.X < 300) ? Position.X : Position.X - 70, 60);

            spriteBatch.DrawString(Map.Instance.GameFont, $"{healthPoints} life left", healthTextPosition, Color.White);
            spriteBatch.DrawString(Map.Instance.GameFont, $"{currentPoints} points", pointsTextPosition, Color.White);
        }

        public void HandleClientIntent(GameTime gameTime, MPKeyPress intent)
        {
            switch (intent)
            {
                case MPKeyPress.None:
                    break;

                case MPKeyPress.Up:

                    if (HitBox.Y > 0)
                    {
                        Position.Y -= (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    break;

                case MPKeyPress.Down:

                    if (HitBox.Y < Game1.Instance.GraphicsDevice.Viewport.Height - HitBox.Height)
                    {
                        Position.Y += (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    break;
            }
        }

        public void ClientUpdateStatsFromServer(int points, int health)
        {
            this.currentPoints = points;
            this.healthPoints = health;
        }

        public MPKeyPress ClientIntent()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                return MPKeyPress.Up;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                return MPKeyPress.Down;
            }

            return MPKeyPress.None;
        }

        public void AddPoints(int points)
        {
            this.currentPoints += points;
        }

        public void DeductHealth()
        {
            this.healthPoints -= 1;
        }
        #endregion

        #region PRIVATE FUNCTIONS
        private void Translate(float deltaTime)
        {
            switch (keyPress)
            {
                case MPKeyPress.None:
                    break;

                case MPKeyPress.Up:

                    if (Position.Y > 0)
                    {
                        Position.Y -= (float)(Speed * deltaTime);
                    }
                    break;

                case MPKeyPress.Down:

                    if (Position.Y < Game1.Instance.GraphicsDevice.Viewport.Height)
                    {
                        Position.Y += (float)(Speed * deltaTime);
                    }
                    break;
            }
        }
        #endregion
    }
}
