using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebServer
{
    public static class TextDatabaseManager
    {
        private static string folderPath = @"C:\pong-server";
        private static string userFileName = @"users.txt";
        private static string highscoreFileName = @"highscore.txt";
        
        public static bool UserExistsInTextFile(string line)
        {
            string fullPath = folderPath + "/" + userFileName;

            // Create folder if not exists
            System.IO.Directory.CreateDirectory(folderPath);

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

        public static List<SavedHighscore> FindTopFiveInHighscore()
        {
            string fullPath = folderPath + "/" + highscoreFileName;
            List<SavedHighscore> tempList = new List<SavedHighscore>();
            List<SavedHighscore> returnList = new List<SavedHighscore>();
            int topFive = 5;

            // Create folder if not exists
            System.IO.Directory.CreateDirectory(folderPath);

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

        public static bool SaveUserInTextFile(string line)
        {
            string fullPath = folderPath + "/" + userFileName;

            return SaveLineInTextFile(line, fullPath);
        }

        public static bool SaveHighscoreInTextFile(string line)
        {
            string fullPath = folderPath + "/" + highscoreFileName;

            return SaveLineInTextFile(line, fullPath);
        }

        private static bool SaveLineInTextFile(string line,  string fullPath)
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