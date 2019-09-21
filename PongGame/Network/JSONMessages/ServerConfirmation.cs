using System.Runtime.Serialization;

namespace PongGame.Network.JSONMessages
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ServerConfirmation
    {
        #region PUBLIC PROPERTIES
        public bool ServerSuccess
        {
            get => this.serverSuccess;
        }
        #endregion

        #region PRIVATE FIELDS
        [DataMember]
        private bool serverSuccess;
        #endregion

        #region CONSTRUCTERS
        public ServerConfirmation(bool serverSuccess)
        {
            this.serverSuccess = serverSuccess;
        }
        #endregion
    }
}
