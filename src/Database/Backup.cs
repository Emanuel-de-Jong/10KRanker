using GlobalValues;
using System;
using System.IO;
using System.Timers;

namespace Database
{
    public static class Backup
    {
        private static Timer makeDBBackupTimer;
        public static int BackupCount { get; set; } = 5;
        public static string BackupDirPath { get; } = G.AssetPath + $"{G.DS}backups";

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
            if (!RemoveOldBackup())
                return;

            File.Copy(Context.DBPath, BackupDirPath + $"{G.DS}10KRanker { DateTime.Now.ToString("MM-dd-y HH-mm") }.db", true);
        }

        private static bool RemoveOldBackup()
        {
            string[] filePaths = Directory.GetFiles(BackupDirPath, "*.db");

            if (filePaths.Length < BackupCount)
                return true;

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

            if (new FileInfo(Context.DBPath).Length == new FileInfo(oldestFilePath).Length)
                return false;

            File.Delete(oldestFilePath);
            return true;
        }

        public static void ClearBackups()
        {
            string[] filePaths = Directory.GetFiles(BackupDirPath, "*.db");

            foreach (string filePath in filePaths)
                File.Delete(filePath);
        }
    }
}
