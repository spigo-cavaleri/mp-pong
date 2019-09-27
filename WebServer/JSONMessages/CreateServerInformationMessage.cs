using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebServer
{
    /// <summary>
    /// Class used for JSON requests regarding hosting a game server
    /// </summary>
    [DataContract] public class CreateServerInformationMessage
    {
        /// <summary>
        /// The IP other players should connect to
        /// </summary>
        public string GameServerIP
        {
            get => this.gameServerIP;
        }

        /// <summary>
        /// The port other players should connect through
        /// </summary>
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