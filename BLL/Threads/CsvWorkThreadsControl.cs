using BLL.Configurations;
using System;
using System.Collections.Generic;

namespace BLL.Threads
{
    public class CsvWorkThreadsControl
    {
        private static object locker = new object();
        internal static List<CsvWorkThread> lstThread = new List<CsvWorkThread>();

        private static SaleTitleConfiguration cfg = new SaleTitleConfiguration();
        private static readonly string[] FILE_NAME_STRUCT = { cfg.ManagerSing, cfg.DataTimeSing };
        private static readonly string[] FILE_DATA_STRUCT = { cfg.DataTimeSing, cfg.ClientSing, cfg.ProductSing, cfg.SumSing };

        public event EventHandler<string> SendMessage;
        private void OnSendMessage(string msg)
        {
            SendMessage?.Invoke(this, msg);
        }


        /// <summary>
        /// Run file handler
        /// </summary>
        /// <param name="file"></param>
        public void Start(string file)
        {
            lock (locker)
            {
                CsvWorkThread csvWT = CreateFileHandlerThread();
                try
                {
                    if (csvWT.Start(file))
                    {
                        OnSendMessage($"{file}: Processing of file starting");
                        lstThread.Add(csvWT);
                        OnSendMessage($"Number of file handler threads - {lstThread.Count}");
                    }
                    else
                    {
                        OnSendMessage($"{file}: can't starting");
                    }
                }
                catch (Exception)
                {
                    OnSendMessage($"{file}: Error starting");
                }
            }
        }

        private CsvWorkThread CreateFileHandlerThread()
        {
            lock (locker)
            {
                CsvWorkThread csvWT = new CsvWorkThread(fns: FILE_NAME_STRUCT, fds: FILE_DATA_STRUCT);
                csvWT.WorkCompleted += CsvWT_WorkCompleted;
                csvWT.ErrorEvent += CsvWT_ErrorEvent;
                return csvWT;
            }
        }



        #region CSVWORKTHREAD_EVENTS
        //##################################################################################################

        private void CsvWT_WorkCompleted(object sender, bool aborted)
        {
            lock (locker)
            {
                var csvWT = sender as CsvWorkThread;
                if (csvWT != null)
                {
                    if (aborted) { OnSendMessage($"{csvWT.Name}: Processing aborted"); }
                    else { OnSendMessage($"{csvWT.Name}: Processing completed"); }

                    csvWT.WorkCompleted -= CsvWT_WorkCompleted;
                    csvWT.ErrorEvent -= CsvWT_ErrorEvent;

                    lstThread.Remove(csvWT);
                    OnSendMessage($"Number of file handler threads - {lstThread.Count}");
                }
            }

        }

        private void CsvWT_ErrorEvent(object sender, string msg)
        {
            lock (locker)
            {
                OnSendMessage($"{(sender as CsvWorkThread)?.Name}: " + msg);
            }
        }

        #endregion // CSVWORKTHREAD_EVENTS



    }
}
