﻿using System;
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


        public override void Update(GameTime gameTime)
        {
 

            
            if (Keyboard.GetState().IsKeyDown(Keys.Up)&& Position.Y >0)
            {
               Position.Y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && Position.Y < 370)
            {
               Position.Y += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            }
           

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (rectangle.Intersects(Ball.rectangle))
            {

                int sf = 33;

                if (Ball.Velocity.X < 0 && Name =="pad1")
                {
                    Ball.Velocity.X = System.Math.Abs(Ball.Velocity.X);
                }
                if (Ball.Velocity.X > 0 && Name == "pad2")
                {
                    Ball.Velocity.X = -System.Math.Abs(Ball.Velocity.X);
                }



            }

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
