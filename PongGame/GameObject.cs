using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    public abstract class GameObject
    {
        public Vector2 Position;
        public Texture2D Sprite;

        public GameObject(Texture2D sprite, Vector2 position)
        {
            Position = position;

            Sprite = sprite;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
