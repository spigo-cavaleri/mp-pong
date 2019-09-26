﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;

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
        public LoginResponseMessage Post()
        {
            string value = Request.Content.ReadAsStringAsync().Result;
            bool wasCreated = false;
            LoginResponseMessage loginResponseMessage;

            if (value == null)
            {
                // Håndter at der ikke sendes noget :)
                // return "WHAT?!?!?!";
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
                SavedServerObject gameServer = FindAvailableGameServer();

                if (gameServer == default(SavedServerObject))
                {
                    loginResponseMessage = new LoginResponseMessage(true, true);
                }
                else
                {
                    gameServer.PlayerCount += 1;
                    loginResponseMessage = new LoginResponseMessage(true, false, gameServer.IP, gameServer.Port);
                }

            }
            else
            {
                loginResponseMessage = new LoginResponseMessage(false);
            }

            return loginResponseMessage;
        }

        private SavedServerObject FindAvailableGameServer()
        {
            return InMemoryDatabase.SavedServerObjects.Find(item => !item.GameFull);
        }
    }
}
