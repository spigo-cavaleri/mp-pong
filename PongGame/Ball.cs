using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
  public  class Ball :GameObject
    {

      public static Rectangle rectangle; // rectangle
      public Vector2 Direction; // Direction 
      public static float speed =7; //fart
      public static Vector2 Velocity;
      public static bool DeadLeft = false;
      public static bool DeadRight = false;

        public  Ball(Texture2D sprite, Vector2 position):base(sprite,position)
        {            
            rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            Sprite = sprite;          
            Position = position;
            SetRandomDirection();
            Velocity = Direction * speed; /// Balls Velocity
        }
        /// <summary>
        /// Giver bolden en random direction mod hæjre eller venstre
        /// </summary>
        public void SetRandomDirection()
        {
           
           Random random = new Random();
            if (random.Next(0, 100) >= 50)
            {
                Direction = new Vector2((random.Next(50, 200)), (random.Next(10, 100) * 2 - 100)); //random ´right
            }
            else
            {
                Direction = new Vector2((random.Next(0, 150) - 200), (random.Next(10, 100) * 2 - 100)); //random left
            }
            Direction.Normalize(); //Normalizes vector so that it is only a unit vector
        }
        public void SetRandomDirectionRight() // sætte velecity random rightside
        {
            Random random = new Random();
            Direction = new Vector2((random.Next(50, 200)), (random.Next(10, 100) * 2 - 100)); //random ´right
            Direction.Normalize();
            Velocity = Vector2.Zero;
            Velocity = Direction * speed;
        }
        public void SetRandomDirectionleft() // sets direction of the ball random on the left side
        {

            Random random = new Random();
            Direction = new Vector2((random.Next(0, 150) - 200), (random.Next(10, 100) * 2 - 100)); //random left
            Direction.Normalize();
            Velocity = Vector2.Zero;
            Velocity = Direction * speed;
        }

        public void BallDead() // Når bolden er ryger in på en af siderne sætter bolden til midten og ryger random mod en side som den ryg in mod
        {

            if (DeadLeft == true)
            {
                Position = new Vector2(640, 360);
                SetRandomDirectionleft();
                Velocity = Direction * speed;
                DeadLeft = false;
            }
            if (DeadRight == true)
            {
                Position = new Vector2(640, 360);
                SetRandomDirectionRight();
                Velocity = Direction * speed;
                DeadRight = false;
            }
        }
        public override void Update(GameTime gameTime)
        {

            if (Game1.Instance.WonGame == false)
            {

                BallDead();
                Position += Velocity;
            }
            else
            {
                Position = new Vector2(Game1.screenWithe / 2, Game1.screenHeight / 2);
            }
           base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch) /// tagner debug regtagle 
        {
            rectangle.X = (int)Position.X;
            rectangle.Y = (int)Position.Y;
            //Rectangle topLine = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
            //Rectangle bottomLine = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
            //Rectangle rightLine = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height);
            //Rectangle leftLine = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);

            //spriteBatch.Draw(Sprite, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(Sprite, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(Sprite, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            //spriteBatch.Draw(Sprite, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

    }   

    
}
