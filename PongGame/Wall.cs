using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame
{
    public class Wall:GameObject
    {


        Rectangle rectangle;
        public string Name;

        public Wall(Texture2D sprite, Vector2 position, float width, float Hight, string name)
        {
            Sprite = sprite;
            Position = position;
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)width, (int)Hight);
            //rectangle = new Rectangle((int)Position.X, (int)Position.Y,300,300);
            Name = name;
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);  
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (rectangle.Intersects(Ball.rectangle))
            {


                if (Ball.Velocity.X > 0 && Name == "WallRight")
                {
                    Ball.Velocity.X = -System.Math.Abs(Ball.Velocity.X);
                }

                if (Ball.Velocity.X < 0 && Name == "WallLeft")
                {
                    Ball.Velocity.X = System.Math.Abs(Ball.Velocity.X);
                }
                if (Ball.Velocity.Y < 0 && Name == "WallNed")
                {
                    Ball.Velocity.Y = System.Math.Abs(Ball.Velocity.Y);
                }

                if (Ball.Velocity.Y > 0 && Name == "WallUp")
                {
                    Ball.Velocity.Y = -Ball.Velocity.Y;
                }



            }
            Rectangle topLine = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
            Rectangle bottomLine = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
            Rectangle rightLine = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height);
            Rectangle leftLine = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);

            spriteBatch.Draw(Sprite, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }


    }
}
