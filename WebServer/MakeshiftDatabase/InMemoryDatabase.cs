using System.Collections.Generic;

namespace WebServer
{
    /// <summary>
    /// Static class for handling Beacon server objects
    /// </summary>
    public static class InMemoryDatabase
    {
        /// <summary>
        /// List of all servers
        /// </summary>
        public static List<SavedServerObject> SavedServerObjects = new List<SavedServerObject>();
    }

    /// <summary>
    /// Beacon server object with information regarding each play server
    /// </summary>
    public class SavedServerObject
    {
        /// <summary>
        /// IP for clients to connect to
        /// </summary>
        public string IP
        {
            get => this.ip;
        }

        /// <summary>
        /// Port for clients to connect through
        /// </summary>
        public string Port
        {
            get => this.port;
        }

        /// <summary>
        /// Amount of players on this server
        /// </summary>
        public int PlayerCount
        {
            get => this.playerCount;
            set { this.playerCount = value; }
        }

        /// <summary>
        /// Whether this game server is full
        /// </summary>
        public bool GameFull
        {
            get => this.playerCount >= 2;
        }

        private string ip;
        private string port;
        private int playerCount;
        private bool gameStarted;

        /// <summary>
        /// Creates a new instance of a game server for beacon purposes
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host port</param>
        public SavedServerObject(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
            this.playerCount = 1;
            this.gameStarted = false;
        }

        
    }
}