using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame.GamePong
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
        #region PUBLIC PROPERTIES
        /// <summary>
        /// true if this pad is running on the server, false otherwise 
        /// </summary>
        public bool IsServer
        {
            get;
            set;
        } = true;

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

        #region CONSTRUCTERS
        /// <summary>
        /// Construct a pad
        /// </summary>
        /// <param name="sprite">The sprite used for the pad</param>
        /// <param name="position">The position of the pad</param>
        /// <param name="name">the name of the pad</param>
        public Pad(Texture2D sprite, Vector2 position, string name) : base(sprite, position)
        {
            Name = name;
            Sprite = sprite;
            Position = position;
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            if (IsServer)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && Position.Y > 0)
                {
                    Position.Y -= (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && Position.Y < Game1.Instance.GraphicsDevice.Viewport.Height)
                {
                    Position.Y += (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            else
            {
                Translate((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
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
