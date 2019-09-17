using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;

namespace WebServer.Controllers
{
    public class SubmitHighscoreController : ApiController
    {
        // GET: api/SubmitHighscore
        public IEnumerable<string> Get()
        {
            return new string[] { "Ingen GET requests er mulige" };
        }

        // GET: api/SubmitHighscore/5
        public string Get(int id)
        {
            return "Ingen GET requests er mulige";
        }

        // POST: api/SubmitHighscore
        public string Post([FromBody]string value)
        {
            bool saved = false;
            string returnString;

            if (value == null)
            {
                // Håndter at der ikke sendes noget :)
            }

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(value));

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SavedHighscore));

                SavedHighscore msg = (SavedHighscore)serializer.ReadObject(ms);

                saved = TextDatabaseManager.SaveHighscoreInTextFile(msg.ToString());

            }
            catch (Exception e)
            {

                throw e; // Håndter errors anderledes ;)
            }

            if(saved)
            {
                returnString = "{\"succes\": true}";
            }
            else
            {
                returnString = "{\"succes\": false}";
            }

            return returnString;
        }
    }
}
