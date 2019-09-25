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
            get => this.userName;
            set { this.userName = value; }

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
        private string userName;

        [DataMember]
        private string password;
        #endregion

        public UserInformationMessage(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        #region PUBLIC FUNCTIONS
        public override string ToString()
        {
            return this.userName + ";" + this.password;
        }
        #endregion
    }
}