using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace PongGame
{
    static class RequestHTTP
    {
        private static string baseUrl = "http://localhost:49823/";

        private static WebResponse SendNRecieve(string where, string what)
        {
            WebRequest request = WebRequest.Create(baseUrl + where);
            request.Method = "POST";

            byte[] msgToSend = System.Text.Encoding.UTF8.GetBytes(what);
            request.ContentLength = msgToSend.Length;
            request.ContentType = "application/json";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(msgToSend, 0, msgToSend.Length);
            dataStream.Close();

            return request.GetResponse();
        }

        public static string Ser<T>(T minType)
        {
            MemoryStream mStream = new MemoryStream();
            DataContractJsonSerializer dJsonSer = new DataContractJsonSerializer(typeof(T));

            dJsonSer.WriteObject(mStream, minType);

            mStream.Position = 0;
            StreamReader sReader = new StreamReader(mStream);
            string requestString = sReader.ReadToEnd();

            return requestString;
        }

        public static LoginResponseMessage CreateAccount(string name, string password)
        {   
            UserInformationMessage uIF = new UserInformationMessage();

            uIF.Username = name;
            uIF.Password = password;

            string requestString = Ser<UserInformationMessage>(uIF);

            WebResponse response = SendNRecieve("api/createuser", requestString);

            using (Stream dataStream = response.GetResponseStream())
            {
                DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(LoginResponseMessage));
                LoginResponseMessage lRM = (LoginResponseMessage)lRMSer.ReadObject(dataStream);
                return lRM;
            }

            //MemoryStream mStream = new MemoryStream();
            //DataContractJsonSerializer dJsonSerializer = new DataContractJsonSerializer(typeof(UserInformationMessage));

            //dJsonSerializer.WriteObject(mStream, uIF);

            //mStream.Position = 0;
            //StreamReader sReader = new StreamReader(mStream);

            //string requestString = sReader.ReadToEnd();
            //byte[] msgToSend = System.Text.Encoding.UTF8.GetBytes(requestString);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + "api/createuser");
            ////request.Credentials = CredentialCache.DefaultCredentials;
            //request.Method = "POST";
            //request.ContentType = "application/json";
            //request.ContentLength = msgToSend.Length;


            //Stream dataStream = request.GetRequestStream();
            //dataStream.Write(msgToSend, 0, msgToSend.Length);
            ////dataStream.Close();

            //WebResponse response = request.GetResponse();
            //// HRFRA
            ////Stream responseStream = response.GetResponseStream();
            ////StreamReader responseReader = new StreamReader(responseStream);

            ////string responseAsJson = responseReader.ReadToEnd();
            ////Console.WriteLine();
            //// HERTIL

            //Stream responseStream = response.GetResponseStream();
            //StreamReader responseReader = new StreamReader(responseStream);

            //string jsonResponse = responseReader.ReadToEnd();

            //MemoryStream jsonMS = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
            //DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(LoginResponseMessage));

            //LoginResponseMessage lRM = (LoginResponseMessage)lRMSer.ReadObject(jsonMS);
            //dataStream.Close();
            //responseStream.Close();
            //jsonMS.Close();
            //response.Close();

            //return lRM;

            //using(dataStream = response.GetResponseStream())
            //{
            //    DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(LoginResponseMessage));
            //    LoginResponseMessage lRM = (LoginResponseMessage)lRMSer.ReadObject(dataStream);
            //    return lRM;
            //}
        }

        public static LoginResponseMessage LogInToAccount(string name, string password)
        {
            //WebRequest request = WebRequest.Create(baseUrl + "api/login");
            ////request.Credentials = CredentialCache.DefaultCredentials;
            //request.Method = "POST";

            UserInformationMessage uIF = new UserInformationMessage();

            uIF.Username = name;
            uIF.Password = password;

            //MemoryStream mStream = new MemoryStream();
            //DataContractJsonSerializer dJsonSerializer = new DataContractJsonSerializer(typeof(UserInformationMessage));

            //dJsonSerializer.WriteObject(mStream, uIF);

            //mStream.Position = 0;
            //StreamReader sReader = new StreamReader(mStream);

            //string requestString = sReader.ReadToEnd();
            //byte[] msgToSend = System.Text.Encoding.UTF8.GetBytes(requestString);
            //request.ContentLength = msgToSend.Length;
            //request.ContentType = "application/json";
            //Stream dataStream = request.GetRequestStream();
            //dataStream.Write(msgToSend, 0, msgToSend.Length);
            //dataStream.Close();

            string requestString = Ser<UserInformationMessage>(uIF);



            WebResponse response = SendNRecieve("api/login", requestString);

            using (Stream dataStream = response.GetResponseStream())
            {
                DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(LoginResponseMessage));
                LoginResponseMessage lRM = (LoginResponseMessage)lRMSer.ReadObject(dataStream);
                return lRM;
            }
        }

        /* 
           send highscore
           host send
        */
        
        public static ServerConfirmation HostServer(string IP, string Port)
        {
            //WebRequest request = WebRequest.Create(baseUrl + "api/serverhost");
            ////request.Credentials = CredentialCache.DefaultCredentials;
            //request.Method = "POST";

            CreateServerInformationMessage cServer = new CreateServerInformationMessage(IP, Port);

            //MemoryStream mStream = new MemoryStream();
            //DataContractJsonSerializer dJsonSerializer = new DataContractJsonSerializer(typeof(CreateServerInformationMessage));

            //dJsonSerializer.WriteObject(mStream, cServer);

            //mStream.Position = 0;
            //StreamReader sReader = new StreamReader(mStream);

            //string requestString = sReader.ReadToEnd();
            //byte[] msgToSend = System.Text.Encoding.UTF8.GetBytes(requestString);
            //request.ContentLength = msgToSend.Length;
            //request.ContentType = "application/json";
            //Stream dataStream = request.GetRequestStream();
            //dataStream.Write(msgToSend, 0, msgToSend.Length);
            //dataStream.Close();

            string requestString = Ser<CreateServerInformationMessage>(cServer);

            WebResponse response = SendNRecieve("api/serverhost", requestString);

            using (Stream dataStream = response.GetResponseStream())
            {
                DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(ServerConfirmation));
                ServerConfirmation lRM = (ServerConfirmation)lRMSer.ReadObject(dataStream);
                return lRM;
            }
        }

        public static ServerConfirmation SendHighscore(string name, int score)
        {
            //WebRequest request = WebRequest.Create(baseUrl + "api/submithighscore");
            ////request.Credentials = CredentialCache.DefaultCredentials;
            //request.Method = "POST";

            SavedHighscore sHS = new SavedHighscore(name, score);

            //MemoryStream mStream = new MemoryStream();
            //DataContractJsonSerializer dJsonSerializer = new DataContractJsonSerializer(typeof(SavedHighscore));

            //dJsonSerializer.WriteObject(mStream, sHS);

            //mStream.Position = 0;
            //StreamReader sReader = new StreamReader(mStream);

            //string requestString = sReader.ReadToEnd();
            //byte[] msgToSend = System.Text.Encoding.UTF8.GetBytes(requestString);
            //request.ContentLength = msgToSend.Length;
            //request.ContentType = "application/json";
            //Stream dataStream = request.GetRequestStream();
            //dataStream.Write(msgToSend, 0, msgToSend.Length);
            //dataStream.Close();

            string requestString = Ser<SavedHighscore>(sHS);

            WebResponse response = SendNRecieve("api/submithighscore", requestString);

            using (Stream dataStream = response.GetResponseStream())
            {
                DataContractJsonSerializer lRMSer = new DataContractJsonSerializer(typeof(ServerConfirmation));
                ServerConfirmation lRM = (ServerConfirmation)lRMSer.ReadObject(dataStream);
                return lRM;
            }
        }

        //public void DoMagic()
        //{
        //    WebRequest request = WebRequest.Create("https://my-json-server.typicode.com/typicode/demo/comments");
        //    request.Method = "POST";

        //    string postData = "This is a test that posts this string to a server";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = byteArray.Length;

        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse response = request.GetResponse();

        //    using (dataStream = response.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(dataStream);
        //        string responseFromServer = reader.ReadToEnd();
        //    }
        //}
    }
}
