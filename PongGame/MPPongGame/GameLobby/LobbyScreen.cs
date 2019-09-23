using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PongGame.Network;
using PongGame.Network.JSONMessages;
using PongGame.Network.Tcp;


namespace PongGame.MPPongGame.GameLobby
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
            if (Map.Instance.IsServer)
            {
                if (GameServer.Instance.GetGameClients().Length > 0)
                {
                    Game1.Instance.GameState = GameState.Playing;
                }
            }
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

                ServerConfirmation sRespone = RequestHTTP.HostServer(GameServer.Instance.LocalEndPoint.Address.ToString(), GameServer.Instance.LocalEndPoint.Port.ToString());
                if (sRespone.ServerSuccess)
                {
                    Map.Instance.SetupAsServer();
                }
                else
                {
                    throw new Exception("SHIIIT");
                }
            }
            else if (!lrm.ShouldHostServer)
            {
                string ip = lrm.GameServerIP;
                string port = lrm.GameServerPort;

                Map.Instance.SetupAsClient(ip, port);
            }

        }

    }
}
