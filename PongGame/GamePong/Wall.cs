using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame.GamePong
{
    public class Wall : GameObject
    {
        public string Name
        {
            get;
            set;
        } = "Wall";

        public GameObject CollidingObject
        {
            get;
            set;
        } = null;

        private int height = 0;
        private int width = 0;
        
        /// <summary>
        /// Constructs a wall
        /// </summary>
        /// <param name="sprite">The sprite of the wall</param>
        /// <param name="position">The position of the wall</param>
        /// <param name="name"></param>
        public Wall(Texture2D sprite, Vector2 position, string name) : base(sprite, position)
        {
            Name = name;
        }

        /// <summary>
        /// Sets the height of the wall
        /// </summary>
        /// <param name="value">The height of the wall</param>
        public void SetHeight(int value)
        {
            height = value;
        }

        /// <summary>
        /// Sets the width of the wall
        /// </summary>
        /// <param name="value">The width of the wall</param>
        public void SetWidth(int value)
        {
            width = value;
        }

        public override void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw from</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected override int GetHeight()
        {
            if (Sprite != null)
            {
                return base.GetHeight();
            }

            return height;
        }

        protected override int GetWidth()
        {
            if (Sprite != null)
            {
                return base.GetWidth();
            }

            return width;
        }
    }
}
