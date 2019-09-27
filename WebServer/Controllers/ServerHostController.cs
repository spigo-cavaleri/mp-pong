using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;

namespace WebServer.Controllers
{
    public class ServerHostController : ApiController
    {
        /// <summary>
        /// GET: api/ServerHost
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "Ingen GET requests er mulige" };
        }

        /// <summary>
        /// GET: api/ServerHost/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "Ingen GET requests er mulige";
        }

        /// <summary>
        /// POST: api/ServerHost
        /// </summary>
        /// <returns></returns>
        public ServerConfirmation Post()
        {
            string value = Request.Content.ReadAsStringAsync().Result;
            bool saved = false;

            if (value == null)
            {
                // Håndter at der ikke sendes noget :)
            }

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(value));

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CreateServerInformationMessage));

                CreateServerInformationMessage msg = (CreateServerInformationMessage)serializer.ReadObject(ms);

                SavedServerObject newServer = new SavedServerObject(msg.GameServerIP, msg.GameServerPort);
                InMemoryDatabase.SavedServerObjects.Add(newServer);

                saved = true;
            }
            catch (Exception e)
            {

                throw e; // Håndter errors anderledes ;)
            }

            if (saved)
            {
                return new ServerConfirmation(true);
            }
            return new ServerConfirmation(false);
        }

    }
}
