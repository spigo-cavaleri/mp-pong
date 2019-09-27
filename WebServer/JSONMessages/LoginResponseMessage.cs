using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebServer
{
    /// <summary>
    /// class used for JSON response messages regarding login / account creation
    /// </summary>
    [DataContract] public class LoginResponseMessage
    {
        /// <summary>
        /// Whether the login/account creation was succesful
        /// </summary>
        public bool LoginSuccesful
        {
            get => this.loginSuccesful;
        }

        /// <summary>
        /// Whether the user should host a new server or play as client
        /// </summary>
        public bool ShouldHostServer
        {
            get => this.shouldHostServer;
        }

        /// <summary>
        /// The IP to connect to, if playing as client
        /// </summary>
        public string GameServerIP
        {
            get => this.gameServerIP;
        }

        /// <summary>
        /// The Port to connect to, if playing as client
        /// </summary>
        public string GameServerPort
        {
            get => this.gameServerPort;
        }

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

        [DataMember] bool loginSuccesful;
        [DataMember] bool shouldHostServer;

        [DataMember] string gameServerIP;
        [DataMember] string gameServerPort;
    }
}