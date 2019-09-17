using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebServer
{
    [DataContract]
    public class SavedHighscore
    {
        public string Username
        {
            get => this.username;
        }
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