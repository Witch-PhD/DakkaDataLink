using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    internal class GlobalLogger
    {
        private static GlobalLogger? m_instance;
        public static GlobalLogger Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new GlobalLogger();
                    m_instance.dataManager = DataManager.Instance;
                }
                return m_instance;
            }
            private set { }
        }
        private GlobalLogger()
        {
            logQueue = new Queue<string>();
            loggerThread = new Thread(loggerThreadTask);
            loggerThread.IsBackground = false;
            loggerThread.Start();
        }

        private DataManager dataManager;
        private Thread loggerThread;

        private static readonly object queueLock = new object();
        private Queue<string> logQueue;
        private bool m_RunLogger = true;

        private void loggerThreadTask()
        {
            using (StreamWriter outputFile = new StreamWriter(dataManager.userOptions.LoggerFilePath))
            {
                outputFile.Write(""); // Clear the log file.
            }
            while (m_RunLogger)
            {
                lock (queueLock)
                {
                    using (StreamWriter outputFile = new StreamWriter(dataManager.userOptions.LoggerFilePath, true))
                    {
                        while (logQueue.Count > 0)
                        {
                            string nextLogLine = logQueue.Dequeue();
                            outputFile.WriteLine(nextLogLine);
                        }
                    }
                }
                Thread.Sleep(250);
            }
        }

        public static void Shutdown()
        {
            m_instance.m_RunLogger = false;
            Thread.Sleep(500);
            m_instance.FlushLog();
        }

        private void FlushLog()
        {
            lock (queueLock)
            {
                using (StreamWriter outputFile = new StreamWriter(dataManager.userOptions.LoggerFilePath, true))
                {
                    while (logQueue.Count > 0)
                    {
                        string nextLogLine = logQueue.Dequeue();
                        outputFile.WriteLine(nextLogLine);
                    }
                }
            }
        }

        public static void Log(string message, bool appendTimestamp = true)
        {
            lock (queueLock)
            {
                if (appendTimestamp)
                {
                    DateTime dateTime = DateTime.Now;
                    string timeStampedLog = $"({dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}.{dateTime.Millisecond})  {message}";
                    Instance.logQueue.Enqueue(timeStampedLog);
                }
                else
                {
                    Instance.logQueue.Enqueue(message);
                }
            }
        }
    }
}
