﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace WebServer.Controllers
{
    public class CreateUserController : ApiController
    {
        // GET: api/CreateUser
        public IEnumerable<string> Get()
        {
            return new string[] { "Ingen GET requests er mulige" };
        }

        // GET: api/CreateUser/5
        public string Get(int id)
        {
            return "Ingen GET requests er mulige";
        }

        // POST: api/CreateUser
        public string Post([FromBody]string value)
        {
            bool wasCreated = false;
            LoginResponseMessage loginResponseMessage;
            string loginResponse;

            if (value == null)
            {
                // Håndter at der ikke sendes noget :)
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserInformationMessage));

                    UserInformationMessage msg = (UserInformationMessage)serializer.ReadObject(ms);

                    wasCreated = TextDatabaseManager.SaveUserInTextFile(msg.ToString());
                }                
            }
            catch (Exception e)
            {

                throw e; // Håndter errors anderledes ;)
            }

            if (wasCreated)
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