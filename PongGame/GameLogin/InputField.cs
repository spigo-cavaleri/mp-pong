using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    class InputField
    {
        public string InputText
        {
            get => this.inputText;
        }
        public string Type
        {
            get => this.type;
        }

        private string type;
        private bool isSelected = false;
        private string inputText = string.Empty;
        private Texture2D sprite;
        private SpriteFont font;
        private Vector2 position;

        public InputField(string type, Vector2 pos, Texture2D sprite, SpriteFont font)
        {
            this.type = type;
            this.sprite = sprite;
            this.font = font;
            this.position = pos;
        }

        public void Update(GameTime gameTime)
        {
            //if (this.isSelected)
            //{
            //    Keyboard.GetState().IsKeyDown(Keys.Escape);
            //    foreach (Keys key in Keyboard.GetState().GetPressedKeys())
            //    {

            //    }
            //    Keyboard.GetState().
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.position, (this.isSelected) ? Color.Green : Color.White);

            spriteBatch.DrawString(this.font, this.inputText, this.position + new Vector2(20, 20), Color.Black);
        }

        public bool SetSelected(Point p)
        {
            if (p.X > this.position.X &&
                p.X < this.position.X + this.sprite.Width &&
                p.Y > this.position.Y &&
                p.Y < this.position.Y + this.sprite.Height)
            {
                this.isSelected = true;
                return true;
            }

            return false;
        }

        public void RemoveSelected()
        {
            this.isSelected = false;
        }

        public void TextInputHandler(object sender, TextInputEventArgs args)
        {
            if (!this.isSelected)
            {
                return;
            }

            Keys pressedKey = args.Key;
            char character = args.Character;
            char back = '\b';
            if (pressedKey == Keys.Back || character == back)
            {
                if (this.inputText.Length > 0)
                {
                    this.inputText = this.inputText.Remove(this.inputText.Length - 1);
                }
            }
            else if (char.IsLetterOrDigit(character))
            {
                string nextLetter = character.ToString();
                this.inputText += nextLetter;
            }
        }
    }
}
