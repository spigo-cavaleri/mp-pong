using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Runtime.Serialization.Json;

namespace WebServer.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Login
        public string Post([FromBody]string value)
        {
            bool exists = false;
            LoginResponseMessage loginResponseMessage;
            string loginResponse;

            if (value == null)
            {
                // Håndter at der ikke sendes noget :)
            }

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(value));

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserInformationMessage));

                UserInformationMessage msg = (UserInformationMessage)serializer.ReadObject(ms);

                exists = TextDatabaseManager.UserExistsInTextFile(msg.ToString());

            }
            catch (Exception e)
            {

                throw e; // Håndter errors anderledes ;)
            }

            if (exists)
            {
                loginResponseMessage = new LoginResponseMessage(true, true);
            }
            else
            {
                loginResponseMessage = new LoginResponseMessage(false);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginResponseMessage));

                serializer.WriteObject(ms, loginResponseMessage);

                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);

                loginResponse = sr.ReadToEnd();
            }

            return loginResponse;
        }
        
    }
}
