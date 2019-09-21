using System.Runtime.Serialization;

namespace PongGame.Network.JSONMessages
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public struct UserInformationMessage
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The name of the user
        /// </summary>
        public string Username
        {
            get => this.username;
            set { this.username = value; }

        }

        /// <summary>
        /// The password of the user
        /// </summary>
        public string Password
        {
            get => this.password;
            set { this.password = value; }
        }
        #endregion

        #region PRIVATE FIELDS
        [DataMember]
        private string username;

        [DataMember]
        private string password;
        #endregion

        #region PUBLIC FUNCTIONS
        public override string ToString()
        {
            return this.username + ";" + this.password;
        }
        #endregion
    }
}