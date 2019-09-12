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

      public static Rectangle rectangle;
      public Vector2 Orgin;

      public Random random;
      public static Vector2 Direction;
      static  public Vector2 Velocity;
       
      public static   float speed =4 ;


        private static Ball instance;

        public static Ball Instance
        {
            get
            {
                if( instance== null)
                {
                    instance = new Ball();
                }
                return instance;
            }
            
        }
        
        public Ball()
        {

        }
 

      public  Ball(Texture2D sprite, Vector2 position):base(sprite,position)
        {
           
            rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            Orgin = new Vector2(Sprite.Width, Sprite.Height);
            Sprite = sprite;
            random = new Random();
            Position = position;
            ////Position = new Vector2(random.Next(0, 720), random.Next(0, 400));
            SetRandomDirection();
             Velocity = Direction * speed;
        }

        public void SetRandomDirection()
        {
            random = new Random();
            Direction = new Vector2((random.Next(0, 200)*2 - 200), (random.Next(0, 200) * 2 - 200)); //Set direction vector components to -1 or 1
            Direction.Normalize(); //Normalizes vector so that it is only a unit vector

        }
        public override void Update(GameTime gameTime)
        {
           

            ////Position += new Vector2(1, 0) * speed;
           Position += Velocity;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            rectangle.X =(int)Position.X;
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
        }

    }   

    
}
