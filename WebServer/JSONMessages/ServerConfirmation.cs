using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebServer
{
    /// <summary>
    /// Struct used for JSON messages to confirm whether an action was succesful
    /// </summary>
    [DataContract] public class ServerConfirmation
    {
        [DataMember] bool serverSuccess;

        public bool ServerSuccess
        {
            get => this.serverSuccess;
        }

        public ServerConfirmation(bool serverSuccess)
        {
            this.serverSuccess = serverSuccess;
        }
    }
}
