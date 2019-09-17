using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.IO;

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

        public static bool SaveUserInTextFile(string line)
        {
            string fullPath = folderPath + "/" + userFileName;

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