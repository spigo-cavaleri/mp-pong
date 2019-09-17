using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PongGame
{
    [DataContract]
    public class CreateServerInformationMessage
    {
        public string GameServerIP
        {
            get => this.gameServerIP;
        }

        public string GameServerPort
        {
            get => this.gameServerPort;
        }

        public CreateServerInformationMessage(string gameServerIP, string gameServerPort)
        {
            this.gameServerIP = gameServerIP;
            this.gameServerPort = gameServerPort;
        }

        [DataMember] string gameServerIP;
        [DataMember] string gameServerPort;
    }
}