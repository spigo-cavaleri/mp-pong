using System;
using System.Runtime.Serialization;

namespace PongGame
{
    /// <summary>
    /// Highscore to save on the rest server
    /// </summary>
    [DataContract]
    public class SavedHighscore
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The username
        /// </summary>
        public string Username
        {
            get => this.username;
        }

        /// <summary>
        /// The score
        /// </summary>
        public int Score
        {
            get => this.score;
        }
        #endregion

        #region PRIVATE FIELDS
        [DataMember] private string username;
        [DataMember] private int score;
        #endregion

        #region CONSTRUCTERS
        public SavedHighscore(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
        #endregion

        #region PUBLIC FUNCTIONS
        public override string ToString()
        {
            return username + ":" + score;
        }

        public static SavedHighscore CreateScoreFromSavedLine(string line)
        {
            string[] split = line.Split(':');

            return new SavedHighscore(split[0], Convert.ToInt32(split[1]));
        }
        #endregion
    }
}