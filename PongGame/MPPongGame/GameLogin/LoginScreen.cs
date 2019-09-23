using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PongGame.Network;
using PongGame.Network.JSONMessages;
using PongGame.MPPongGame.GameLobby;

namespace PongGame.MPPongGame.GameLogin
{
    public class LoginScreen
    {
        public static LoginScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginScreen();
                }

                return instance;
            }
        }

        private Texture2D inputTexture;
        private Texture2D loginButtonTexture;
        private Texture2D createButtonTexture;

        private InputField usernameInput;
        private InputField passwordInput;
        private Button loginButton;
        private Button createButton;

        private SpriteFont inputFont;

        private static LoginScreen instance;

        private LoginScreen()
        {

        }

        public void LoadContent()
        {
            inputFont = Game1.Instance.Content.Load<SpriteFont>("InputFont");

            inputTexture = Game1.Instance.Content.Load<Texture2D>("input");
            loginButtonTexture = Game1.Instance.Content.Load<Texture2D>("login");
            createButtonTexture = Game1.Instance.Content.Load<Texture2D>("create");

            usernameInput = new InputField("username", new Vector2(100, 100), inputTexture, inputFont);
            passwordInput = new InputField("password", new Vector2(100, 200), inputTexture, inputFont);
            loginButton = new Button(loginButtonTexture, new Vector2(100, 400));
            createButton = new Button(createButtonTexture, new Vector2(350, 400));

            // Bruges til at hooke op på tastetryk event i input felter
            Game1.Instance.Window.TextInput += usernameInput.TextInputHandler;
            Game1.Instance.Window.TextInput += passwordInput.TextInputHandler;

            // Lambda metoder som kaldes, når knapperne trykkes på skal også initieres
            loginButton.SetButtonDelegate(() => {
                string username = usernameInput.InputText;
                string password = passwordInput.InputText;
                LoginResponseMessage lResponse = RequestHTTP.LogInToAccount(username, password);
                // RequestHTTP.LoginToAccount(username, password);

                Game1.Instance.GameState = GameState.WaitingForPlayer;
                LobbyScreen.Instance.ParseLoginRespone(lResponse);
            });
            createButton.SetButtonDelegate(() => {
                string username = usernameInput.InputText;
                string password = passwordInput.InputText;
                LoginResponseMessage cResponse = RequestHTTP.CreateAccount(username, password);
                // RequestHTTP.CreateAccount(username, password);
                Game1.Instance.GameState = GameState.WaitingForPlayer;
                LobbyScreen.Instance.ParseLoginRespone(cResponse);
            });
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            // CHECK FOR CLICK ON STUFFS
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                usernameInput.RemoveSelected();
                passwordInput.RemoveSelected();

                usernameInput.SetSelected(mouseState.Position);
                passwordInput.SetSelected(mouseState.Position);
                loginButton.CheckForClick(mouseState.Position);
                createButton.CheckForClick(mouseState.Position);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            usernameInput.Draw(spriteBatch);
            passwordInput.Draw(spriteBatch);
            loginButton.Draw(spriteBatch);
            createButton.Draw(spriteBatch);
        }
    }
}
