using System.Runtime.Serialization;

namespace PongGame.Network.JSONMessages
{
    /// <summary>
    /// Holds server information to send to the beacon and to be received by pending client 
    /// </summary>
    [DataContract]
    public class CreateServerInformationMessage
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The game servers Ip address
        /// </summary>
        public string GameServerIP
        {
            get => this.gameServerIP;
        }

        /// <summary>
        /// The game servers port address
        /// </summary>
        public string GameServerPort
        {
            get => this.gameServerPort;
        }
        #endregion

        #region PRIVATE FIELDS
        [DataMember]
        private string gameServerIP;

        [DataMember]
        private string gameServerPort;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a create server information message
        /// </summary>
        /// <param name="gameServerIP">The servers ip address</param>
        /// <param name="gameServerPort">The servers port address</param>
        public CreateServerInformationMessage(string gameServerIP, string gameServerPort)
        {
            this.gameServerIP = gameServerIP;
            this.gameServerPort = gameServerPort;
        }
        #endregion
    }
}