using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebServer
{
    [DataContract] public class LoginResponseMessage
    {
        public bool LoginSuccesful
        {
            get => this.loginSuccesful;
        }

        public bool ShouldHostServer
        {
            get => this.shouldHostServer;
        }

        public string GameServerIP
        {
            get => this.gameServerIP;
        }

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