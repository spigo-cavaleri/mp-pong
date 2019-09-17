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
  public  class Pad : GameObject  
    {

        Rectangle rectangle; 
        public float speed = 400;
        public string Name;      

        public Pad(Texture2D sprite, Vector2 position, string name): base(sprite, position)
        {
            Name = name;
            Sprite = sprite;
            Position = position;
            rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// /// movement af pads
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) 
        {            
            if (Keyboard.GetState().IsKeyDown(Keys.Up)&& Position.Y >0)
            {
               Position.Y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && Position.Y < 620)
            {
               Position.Y += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
            }           
            base.Update(gameTime);
        }
        /// <summary>
        /// /// cheker om bolden retgale rammer pad1 eller pad2
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (rectangle.Intersects(Ball.rectangle))
            {             
                if (Ball.Velocity.X < 0 && Name =="pad1") // Ball Hits Pad One
                {
                    Ball.Velocity.X = System.Math.Abs(Ball.Velocity.X);
                    Ball.Velocity *= 1.04f;
                }
                if (Ball.Velocity.X > 0 && Name == "pad2")// Ball Hits Pad Two
                {
                    Ball.Velocity.X = -System.Math.Abs(Ball.Velocity.X);
                    Ball.Velocity *= 1.04f;
                }
            }

            // sætter retagle positon og teger debug
            rectangle.X = (int)Position.X;
            rectangle.Y = (int)Position.Y;
            Rectangle topLine = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
            Rectangle bottomLine = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
            Rectangle rightLine = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height);
            Rectangle leftLine = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);

            spriteBatch.Draw(Sprite, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(Sprite, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
