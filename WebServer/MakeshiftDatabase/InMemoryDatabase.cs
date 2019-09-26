using System.Collections.Generic;

namespace WebServer
{
    public static class InMemoryDatabase
    {
        public static List<SavedServerObject> SavedServerObjects = new List<SavedServerObject>();
    }

    public class SavedServerObject
    {
        public string IP
        {
            get => this.ip;
        }
        public string Port
        {
            get => this.port;
        }
        public int PlayerCount
        {
            get => this.playerCount;
            set { this.playerCount = value; }
        }
        public bool GameFull
        {
            get => this.playerCount >= 2;
        }

        private string ip;
        private string port;
        private int playerCount;
        private bool gameStarted;

        public SavedServerObject(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
            this.playerCount = 1;
            this.gameStarted = false;
        }

        
    }
}