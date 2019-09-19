using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    public delegate void OnClickEventHandler();
    class Button
    {
        private Texture2D sprite;
        private Vector2 position;
        private OnClickEventHandler onClickEventHandler;

        public Button(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void SetButtonDelegate(OnClickEventHandler d)
        {
            this.onClickEventHandler = d;
        }

        public void CheckForClick(Point p)
        {
            if (p.X > this.position.X &&
                p.X < this.position.X + this.sprite.Width &&
                p.Y > this.position.Y &&
                p.Y < this.position.Y + this.sprite.Height)
            {
                this.onClickEventHandler();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.position, Color.White);
        }
    }
}
