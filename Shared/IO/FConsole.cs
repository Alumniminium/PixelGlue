using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Shared.IO
{
    public static class FConsole
    {
        private static bool _logToFile;
        public static bool LogToFile
        {
            get { return _logToFile; }
            set
            {
                _logToFile = value;
                if (value && _writer == null)
                    _writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/PixelGlueLog.txt", true);
                if (!value && _writer != null)
                { _writer.Dispose(); _writer = null; }
            }
        }
        private static StreamWriter _writer;
        private static readonly AutoResetEvent _sync = new AutoResetEvent(false);
        private static readonly ConcurrentQueue<string> _pendingLines = new ConcurrentQueue<string>();
        private static readonly Thread _workerThread;

        static FConsole()
        {
            _workerThread = new Thread(WorkLoop)
            {
                Name = "FConsole Thread",
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            _workerThread.Start();
        }
        public static void WriteLine(string line)
        {
            _pendingLines.Enqueue(line);
            _sync.Set();
        }
        private static void WorkLoop()
        {
            while (true)
            {
                _sync.WaitOne();
                while (_pendingLines.TryDequeue(out var line))
                {
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " - " + line);
                    if (LogToFile)
                        _writer.WriteLine(DateTime.Now.ToLongTimeString() + " - " + line);
                }
            }
        }
    }
}