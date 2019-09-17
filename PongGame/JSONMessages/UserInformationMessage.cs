using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace PongGame
{
    [DataContract] public struct UserInformationMessage
    {
        public string Username
        {
            get => this.username;
            set { this.username = value; }

        }

        public string Password
        {
            get => this.password;
            set { this.password = value; }
        }

        [DataMember] private string username;
        [DataMember] private string password;

        public override string ToString()
        {
            return this.username + ";" + this.password;
        }
    }
}