using System.Runtime.Serialization;

namespace PongGame.Network.JSONMessages
{
    /// <summary>
    /// A login response message
    /// </summary>
    [DataContract]
    public class LoginResponseMessage
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// True if the login is sucessful, false otherwise
        /// </summary>
        public bool LoginSuccesful
        {
            get => this.loginSuccesful;
        }

        /// <summary>
        /// True if going to host server, false otherwise
        /// </summary>
        public bool ShouldHostServer
        {
            get => this.shouldHostServer;
        }

        /// <summary>
        /// The servers ip address
        /// </summary>
        public string GameServerIP
        {
            get => this.gameServerIP;
        }
        
        /// <summary>
        /// The servers port address
        /// </summary>
        public string GameServerPort
        {
            get => this.gameServerPort;
        }
        #endregion

        #region PRIVATE FIELDS
        [DataMember]
        private bool loginSuccesful;

        [DataMember]
        private bool shouldHostServer;

        [DataMember]
        private string gameServerIP;

        [DataMember]
        private string gameServerPort;
        #endregion

        #region CONSTUCTERS
        /// <summary>
        /// Constructs a login response message
        /// </summary>
        /// <param name="loginSuccesful"></param>
        public LoginResponseMessage(bool loginSuccesful)
        {
            this.loginSuccesful = loginSuccesful;
        }

        public LoginResponseMessage(bool loginSuccesful, bool shouldHostServer)
        {
            this.loginSuccesful = loginSuccesful;
            this.shouldHostServer = shouldHostServer;
        }

        public LoginResponseMessage(bool loginSuccesful, bool shouldHostServer, string gameServerIP, string gameServerPort)
        {
            this.loginSuccesful = loginSuccesful;
            this.shouldHostServer = shouldHostServer;
            this.gameServerIP = gameServerIP;
            this.gameServerPort = gameServerPort;
        }
        #endregion
    }
}