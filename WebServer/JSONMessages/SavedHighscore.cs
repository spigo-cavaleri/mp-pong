using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebServer
{
    /// <summary>
    /// Struct used to for JSON messages regarding sending highscores to API
    /// </summary>
    [DataContract] public struct SavedHighscore
    {
        /// <summary>
        /// The winning user
        /// </summary>
        public string Username
        {
            get => this.username;
        }

        /// <summary>
        /// The winning users score
        /// </summary>
        public int Score
        {
            get => this.score;
        }

        [DataMember] private string username;
        [DataMember] private int score;

        public SavedHighscore(string username, int score)
        {
            this.username = username;
            this.score = score;
        }

        public override string ToString()
        {
            return username + ":" + score;
        }

        public static SavedHighscore CreateScoreFromSavedLine(string line)
        {
            string[] split = line.Split(':');

            return new SavedHighscore(split[0], Convert.ToInt32(split[1]));
        }
    }
}