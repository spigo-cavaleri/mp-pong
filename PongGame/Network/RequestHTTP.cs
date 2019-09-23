using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

using PongGame.Network.JSONMessages;

namespace PongGame.Network
{
    /// <summary>
    /// Controller to send and receive request from the REST server
    /// </summary>
    public static class RequestHTTP
    {
        private static string baseUrl = "http://localhost:49823/";


        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>Respones message from the REST server</returns>
        public static LoginResponseMessage CreateAccount(string name, string password)
        {

            UserInformationMessage uIF = new UserInformationMessage(name, password);

            if (Serializer(uIF, out string requestString))
            {
                if (DeSerializer(SendNRecieve("api/createuser", requestString), out LoginResponseMessage loginResponseMessage))
                {
                    return loginResponseMessage;
                }
            }

            return null;
        }

        /// <summary>
        /// Login to current user
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>Respones message from the REST server</returns>
        public static LoginResponseMessage LogInToAccount(string name, string password)
        {
            UserInformationMessage uIF = new UserInformationMessage(name, password);

            if (Serializer(uIF, out string requestString))
            {
                if (DeSerializer(SendNRecieve("api/login", requestString), out LoginResponseMessage loginResponseMessage))
                {
                    return loginResponseMessage;
                }
            }

            return null;
        }

        /// <summary>
        /// Sends server information to the beacon.
        /// </summary>
        /// <param name="IP">The ip address of the server</param>
        /// <param name="Port">The port address of the server</param>
        /// <returns>Respones message from the REST server</returns>
        public static ServerConfirmation HostServer(string IP, string Port)
        {
            CreateServerInformationMessage cServer = new CreateServerInformationMessage(IP, Port);

            if (Serializer(cServer, out string requestString))
            {
                if (DeSerializer(SendNRecieve("api/serverhost", requestString), out ServerConfirmation serverConfirmation))
                {
                    return serverConfirmation;
                }
            }

            return null;
        }

        /// <summary>
        /// Send highscore to the REST server
        /// </summary>
        /// <param name="name">The name of the user that submints the score</param>
        /// <param name="score">The score</param>
        /// <returns>Respones message from the REST server</returns>
        public static ServerConfirmation SendHighscore(string name, int score)
        {
            SavedHighscore sHS = new SavedHighscore(name, score);

            if (Serializer(sHS, out string requestString))
            {
                if (DeSerializer(SendNRecieve("api/submithighscore", requestString), out ServerConfirmation serverConfirmation))
                {
                    return serverConfirmation;
                }
            }

            return null;
        }


        /// <summary>
        /// Deserializes a response object from the REST server
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="response">The response stream from the REST server</param>
        /// <param name="webRequest">The response object</param>
        /// <returns>True if the response is desialized, false otherwise</returns>
        private static bool DeSerializer<T>(WebResponse response, out T webRequest)
        {
            using (Stream dataStream = response.GetResponseStream())
            {
                DataContractJsonSerializer deSerializer = new DataContractJsonSerializer(typeof(T));
                webRequest = (T)deSerializer.ReadObject(dataStream);

                if (webRequest != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serializes the request data object and returns the data as a string
        /// </summary>
        /// <typeparam name="T">Type of data request object</typeparam>
        /// <param name="reqeustDataObject">The request object</param>
        /// <returns>True if the object is serialized, false otherwise</returns>
        private static bool Serializer<T>(T reqeustDataObject, out string requestString)
        {
            MemoryStream mStream = new MemoryStream();
            DataContractJsonSerializer dJsonSer = new DataContractJsonSerializer(typeof(T));

            dJsonSer.WriteObject(mStream, reqeustDataObject);

            mStream.Position = 0;
            StreamReader sReader = new StreamReader(mStream);
            requestString = sReader.ReadToEnd();

            if (requestString != null)
            {
                return true;
            }

            return false; 
        }

        /// <summary>
        /// Send and Receives data to and from the REST server
        /// </summary>
        /// <param name="requestAddress">The address to send a request to</param>
        /// <param name="requestData">The data in the request</param>
        /// <returns></returns>
        private static WebResponse SendNRecieve(string requestAddress, string requestData)
        {
            WebRequest request = WebRequest.Create(baseUrl + requestAddress);
            request.Method = "POST";

            byte[] msgToSend = Encoding.UTF8.GetBytes(requestData);
            request.ContentLength = msgToSend.Length;
            request.ContentType = "application/json";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(msgToSend, 0, msgToSend.Length);
            dataStream.Close();

            return request.GetResponse();
        }
    }
}
