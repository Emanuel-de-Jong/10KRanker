using System;
using System.IO;
using System.Timers;

namespace Logger
{
    public class Log
    {
        public static string LogDirPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\10KRanked\logs";

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

        public void Write(string message)
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

            writer.WriteLine($"{ DateTime.Now }: { message }");
            writer.Flush();
        }

        private void OnDisposeWriterTimerElapsed(object s, ElapsedEventArgs e)
        {
            writer.Close();
            writer = null;
        }
    }
}
