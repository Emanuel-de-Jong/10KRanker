using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class Backup
    {
        public static int BackupCount { get; set; } = 3;
        public static string BackupDirPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\10KRanked\backups";

        public static void Make()
        {
            RemoveOldBackup();

            File.Copy(Context.DBPath, BackupDirPath + @$"\10KRanked { DateTime.Now.ToString("MM-dd-y HH:mm:ss") }.db", true);
        }

        private static void RemoveOldBackup()
        {
            string[] filePaths = Directory.GetFiles(BackupDirPath, "*.db");

            if (filePaths.Length < BackupCount)
                return;

            DateTime fileDate;
            DateTime oldestFileDate = DateTime.MaxValue;
            string oldestFilePath = null;
            foreach (string filePath in filePaths)
            {
                fileDate = File.GetCreationTime(filePath);

                if (fileDate < oldestFileDate)
                {
                    oldestFileDate = fileDate;
                    oldestFilePath = filePath;
                }
            }

            File.Delete(oldestFilePath);
        }
    }
}
