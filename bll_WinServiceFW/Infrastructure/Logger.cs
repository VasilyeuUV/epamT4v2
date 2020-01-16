using System;
using System.IO;

namespace bll_WinServiceFW.Infrastructure
{
    internal sealed class Logger
    {
        private static object obj = new object();          // some object for lock
        internal static string LogFile { get; set; } = @"d:\fwServiceLogFile.txt";

        internal Logger(string filePath)
        {
            try
            {
                File.Create(filePath);
                LogFile = filePath;
            }
            catch (Exception)
            {
            }
        }


        private void Save(string log)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(LogFile, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")}: {log}");
                    writer.Flush();
                }
            }
        }



        /// <summary>
        /// Save event result
        /// </summary>
        /// <param name="fileEvent">file event</param>
        /// <param name="filePath">file name</param>
        internal void RecordEntry(string fileEvent, string filePath)
        {
            Save($"File {filePath} was {fileEvent}");
        }

        /// <summary>
        /// Save event result
        /// </summary>
        /// <param name="logEvent"></param>
        internal void RecordEvent(string logEvent)
        {
            Save($"{logEvent}");
        }
    }
}
