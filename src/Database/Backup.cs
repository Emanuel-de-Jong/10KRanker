using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Database
{
    public static class Backup
    {
        private static Timer makeDBBackupTimer;
        public static int BackupCount { get; set; } = 5;
        public static string BackupDirPath { get; }
            = Context.DBDirPath + @"\backups";

        public static void Init()
        {
            if (!Directory.Exists(BackupDirPath))
                Directory.CreateDirectory(BackupDirPath);

            makeDBBackupTimer = new Timer(1 * 24 * 60 * 60 * 1000);
            makeDBBackupTimer.AutoReset = true;
            makeDBBackupTimer.Elapsed += OnMakeDBBackupTimerElapsed;
            makeDBBackupTimer.Start();
        }

        public static void OnMakeDBBackupTimerElapsed(object s, ElapsedEventArgs e)
        {
            Make();
        }

        public static void Make()
        {
            RemoveOldBackup();

            File.Copy(Context.DBPath, BackupDirPath + @$"\10KRanked { DateTime.Now.ToString("MM-dd-y HH-mm") }.db", true);
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

        public static void ClearBackups()
        {
            string[] filePaths = Directory.GetFiles(BackupDirPath, "*.db");

            foreach (string filePath in filePaths)
                File.Delete(filePath);
        }
    }
}
