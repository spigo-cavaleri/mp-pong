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

        Rectangle rectangle;
        public Vector2 Orgin;

       public Random random;
       public Vector2 Direction;

      public   float speed =4 ;

      public  Ball(Texture2D sprite, Vector2 position):base(sprite,position)
        {
           
            rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            Orgin = new Vector2(Sprite.Width, sprite.Height);
            Sprite = sprite;
            random = new Random();
            Position = position;
            //Position = new Vector2(random.Next(0, 720), random.Next(0, 400));
            SetRandomDirection();
        }

        public void SetRandomDirection()
        {
            random = new Random();
            Direction = new Vector2((random.Next(0, 200)*2 - 200), (random.Next(0, 200) * 2 - 200)); //Set direction vector components to -1 or 1
            Direction.Normalize(); //Normalizes vector so that it is only a unit vector

        }
        public override void Update(GameTime gameTime)
        {

            Position += Direction * speed;
            base.Update(gameTime);
        }

    }   

    
}
