using System;
using System.Collections.Generic;
using System.IO;

namespace WebServer
{
    /// <summary>
    /// Makeshift plaintext .txt formatted database handler
    /// </summary>
    public static class TextDatabaseManager
    {
        private static string folderPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string userFileName = @"users.txt";
        private static string highscoreFileName = @"highscore.txt";
        
        /// <summary>
        /// Checks for existing users in database file
        /// </summary>
        /// <param name="line">The formatted user;password to search for</param>
        /// <returns>Whether the user is found or not</returns>
        public static bool UserExistsInTextFile(string line)
        {
            string fullPath = Path.Combine(folderPath, userFileName);

            // Create folder if not exists
            Directory.CreateDirectory(folderPath);

            if (! File.Exists(fullPath))
            {
                File.CreateText(fullPath);
            }

            try
            {
                string[] textLines = File.ReadAllLines(fullPath);

                for (int i = 0; i < textLines.Length; i++)
                {
                    if (textLines[i] == line)
                    {
                        return true;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        /// <summary>
        /// Find top 5 highscores in descending order of scores
        /// </summary>
        /// <returns>Top 5 SavedHighscore objects</returns>
        public static List<SavedHighscore> FindTopFiveInHighscore()
        {
            string fullPath = Path.Combine(folderPath, highscoreFileName);
            List<SavedHighscore> tempList = new List<SavedHighscore>();
            List<SavedHighscore> returnList = new List<SavedHighscore>();
            int topFive = 5;

            // Create folder if not exists
            Directory.CreateDirectory(folderPath);

            if (!File.Exists(fullPath))
            {
                File.CreateText(fullPath);
            }

            try
            {
                string[] textLines = File.ReadAllLines(fullPath);

                for (int i = 0; i < textLines.Length; i++)
                {
                    tempList.Add(SavedHighscore.CreateScoreFromSavedLine(textLines[i]));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            tempList.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));

            for (int i = 0; i < tempList.Count; i++)
            {
                if (i < topFive)
                {
                    returnList.Add(tempList[i]);
                }
                else
                {
                    break;
                }
            }

            return returnList;
        }

        /// <summary>
        /// Saves a new user in the database
        /// </summary>
        /// <param name="line">The formatted user;password to save</param>
        /// <returns>Whether the user was saved or not</returns>
        public static bool SaveUserInTextFile(string line)
        {
            string fullPath = Path.Combine(folderPath, userFileName);

            return SaveLineInTextFile(line, fullPath);
        }

        /// <summary>
        /// Saves a new highscore in the database
        /// </summary>
        /// <param name="line">The formatted user;score to save</param>
        /// <returns>Whether the highscore was saved or not</returns>
        public static bool SaveHighscoreInTextFile(string line)
        {
            string fullPath = Path.Combine(folderPath, highscoreFileName);

            return SaveLineInTextFile(line, fullPath);
        }

        private static bool SaveLineInTextFile(string line, string fullPath)
        {
            // Create folder if not exists
            Directory.CreateDirectory(folderPath);

            try
            {
                using (StreamWriter sWriter = File.AppendText(fullPath))
                {
                    sWriter.WriteLine(line);
                    sWriter.Flush();
                }

                return true;
            }
            catch (Exception e)
            {

                throw e;    // Bør udkommenteres og error håndteres anderledes hen ad vejen
            }

            return false;
        }
    }
}