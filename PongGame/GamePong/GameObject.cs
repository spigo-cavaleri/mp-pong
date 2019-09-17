using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame.GamePong
{
    /// <summary>
    /// The base class for all game objects in the game
    /// </summary>
    public abstract class GameObject
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Returns the height of the game object
        /// </summary>
        public int Height
        {
            get { return GetHeight(); }
        }

        /// <summary>
        /// Returns the width of the game object
        /// </summary>
        public int Width
        {
            get { return GetWidth(); }
        }

        /// <summary>
        /// The sprite of the game object
        /// </summary>
        public Texture2D Sprite
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Hitbox for collision
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        #endregion

        #region PUBLIC FIELDS
        /// <summary>
        /// The position of the game object
        /// </summary>
        public Vector2 Position;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a gameobject
        /// </summary>
        public GameObject() : this(null, new Vector2()) { }
       
        /// <summary>
        /// Constructs a game object
        /// </summary>
        /// <param name="sprite">The sprite to use for drawing the game object</param>
        /// <param name="position">The position of the game object</param>
        public GameObject(Texture2D sprite, Vector2 position)
        {
            Position = position;
            Sprite = sprite;
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Constrols if one game object is colliding with another
        /// </summary>
        /// <param name="colliderGameObject">The game object to check against</param>
        /// <returns>True if the gameobjects collides, false otherwise</returns>
        public bool IsColliding(GameObject colliderGameObject)
        {
            if (colliderGameObject == null)
            {
                return false;
            }

            if (HitBox.Intersects(colliderGameObject.HitBox))
            {
                return true;
            }

            return false;
        }  

        /// <summary>
        /// Loads content for monogame
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for drawing game objects</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite, HitBox, Color.White);
            }
        }
        #endregion

        #region PROTECTED FUNCTIONS
        /// <summary>
        /// The height of the game object
        /// </summary>
        /// <returns>if the sprite is null returns 0, otherwise returns the width of the sprite</returns>
        protected virtual int GetWidth()
        {
            if (Sprite != null)
            {
                return Sprite.Width;
            }

            return 0;
        }

        /// <summary>
        /// The height of the game object
        /// </summary>
        /// <returns>if the sprite is null returns 0, otherwise returns the height of the sprite</returns>
        protected virtual int GetHeight()
        {
            if (Sprite != null)
            {
                return Sprite.Height;
            }

            return 0;
        }
        #endregion
    }
}
