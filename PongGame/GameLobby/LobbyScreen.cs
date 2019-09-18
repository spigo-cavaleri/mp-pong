using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    class LobbyScreen
    {
        public static LobbyScreen Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LobbyScreen();
                }
                return instance;
            }
        }

        private SpriteFont waitFont;
        private string loadText;

        private static LobbyScreen instance;

        private LobbyScreen()
        {

        }

        public void LoadContent()
        {
            waitFont = Game1.Instance.Content.Load<SpriteFont>("InputFont");
            loadText = "Waiting For Game :)";

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(waitFont, loadText, new Vector2(250, 250), Color.Beige);
        }

        public void ParseLoginRespone(LoginResponseMessage lrm)
        {
            if (!lrm.LoginSuccesful)
            {
                Game1.Instance.GameState = GameState.LoginScreen;
            }
            else if (lrm.ShouldHostServer)
            {
                //JegSkalHosteEnServer
                ServerConfirmation sRespone = RequestHTTP.HostServer("127.0.0.2", "13000");
                if (sRespone.ServerSuccess)
                {
                    Game1.Instance.GameState = GameState.Playing;
                }
                else
                {
                    throw new Exception("SHIIIT");
                }
            }
            else if (!lrm.ShouldHostServer)
            {
                string iP = lrm.GameServerIP;
                string port = lrm.GameServerPort;

                //JegSkalConnecteTiliPport
                Game1.Instance.GameState = GameState.Playing;
            }

        }

    }
}
