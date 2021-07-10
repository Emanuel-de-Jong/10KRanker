using System;
using System.IO;
using System.Timers;

namespace Logger
{
    public class Log
    {
        public static string LogDirPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\10KRanked\logs";

        private string logName;
        private string logPath;
        private StreamWriter writer;
        private Timer disposeWriterTimer;

        public Log(string logName)
        {
            if (!Directory.Exists(LogDirPath))
                Directory.CreateDirectory(LogDirPath);

            this.logName = logName;
            this.logPath = LogDirPath + @$"\{ logName }.log";

            disposeWriterTimer = new Timer(5000);
            disposeWriterTimer.AutoReset = false;
            disposeWriterTimer.Elapsed += OnDisposeWriterTimerElapsed;
        }

        public void Write(string message, string comment = null)
        {
            if (writer == null)
            {
                writer = new StreamWriter(logPath, true);
            }
            else
            {
                disposeWriterTimer.Stop();
            }
            disposeWriterTimer.Start();

            string line = $"{ DateTime.Now.ToString("MM-dd-y HH:mm:ss") }: { message }";
            if (comment != null)
                line += $" // { comment }";

            writer.WriteLine(line);
            writer.Flush();
        }

        private void OnDisposeWriterTimerElapsed(object s, ElapsedEventArgs e)
        {
            writer.Close();
            writer = null;
        }

        public void ClearLog(bool removeFiles = false)
        {
            if (!removeFiles)
            {
                File.WriteAllText(logPath, "");
            }
            else
            {
                File.Delete(logPath);
            }
        }

        public static void ClearLogs(bool removeFiles = false)
        {
            string[] filePaths = Directory.GetFiles(LogDirPath, "*.log");

            if (!removeFiles)
            {
                foreach (string filePath in filePaths)
                    File.WriteAllText(filePath, "");
            }
            else
            {
                foreach (string filePath in filePaths)
                    File.Delete(filePath);
            }
        }
    }
}
