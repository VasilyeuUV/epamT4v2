using BLL.Models;
using BLL.Parsers;
using System;
using System.Configuration;
using System.Threading;

namespace BLL.Threads
{
    internal class CsvWorkThread
    {
        private static readonly object locker = new object();

        private Thread _thread = null;

        private string[] _fileNameStruct = null;
        private string[] _fileContextStruct = null;

        private bool _abort = false;
        private bool Abort
        {
            get => _abort;
            set
            {
                _abort = value;
                OnWorkCompleting(_abort);
            }
        }

        internal event EventHandler<bool> WorkCompleted;
        internal event EventHandler<string> ErrorEvent;


        /// <summary>
        /// CTOR
        /// </summary>
        public CsvWorkThread(string[] fns, string[] fds)
        {
            this._fileContextStruct = fds;
            this._fileNameStruct = fns;

            this._thread = new Thread(this.RunProcess);
        }


        /// <summary>
        /// Start this Thread
        /// </summary>
        /// <param name="products"></param>
        internal bool Start(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                this._thread.IsBackground = true;
                this._thread?.Start(filePath);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Main thread method
        /// </summary>
        private void RunProcess(object obj)
        {
            string filePath = (string)obj;
            SalesFileNameModel sfnm = null;


            // PARSE FILE NAME
            if (!this.Abort) { return; }
            {
                string[] fileNameData = (new CSVParser()).GetFileNameParsingResult(filePath, ConfigurationManager.AppSettings["FileNamePattern"]);
                lock (locker) { sfnm =  SalesFileNameModel.CreateInstance(filePath, fileNameData); }
            }



            this._fnsm = GetFileNameData(filePath);
            if (this._fnsm == null) { return; }


        }






        /// <summary>
        /// Thread work completed event
        /// </summary>
        /// <param name="abort">true - if thread was being aborted</param>
        /// <param name="deleteTmpData">true - if data was saved to database</param>
        private void OnWorkCompleting(bool abort)
        {
            WorkCompleted?.Invoke(this, abort);
        }


    }
}
