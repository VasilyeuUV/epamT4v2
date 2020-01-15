using pl_FWConsole.Configurations;
using System.Collections.Generic;
using BLL.Threads;
using pl_FWConsole.ConsoleMenus;
using System;

namespace pl_FWConsole.WorkVersions
{
    public static class WorkingProcess
    {
        private static object locker = new object();
        private static SaleConfiguration cfg = new SaleConfiguration();
        internal static List<CsvWorkThread> lstThread = new List<CsvWorkThread>();

        private static readonly string[] FILE_NAME_STRUCT = { cfg.ManagerSing, cfg.DataTimeSing };
        private static readonly string[] FILE_DATA_STRUCT = { cfg.DataTimeSing, cfg.ClientSing, cfg.ProductSing, cfg.SumSing };

        /// <summary>
        /// Run file handler
        /// </summary>
        /// <param name="file"></param>
        public static void Start(string file)
        {
            lock (locker)
            {
                CsvWorkThread csvWT = CreateFileHandlerThread();
                try
                {
                    if (csvWT.Start(file))
                    {
                        Display.Message($"{file}: Processing of file starting");
                        lstThread.Add(csvWT);
                        Display.Message($"Number of file handler threads - {lstThread.Count}", ConsoleColor.Blue);
                    }
                    else
                    {
                        Display.Message($"{file}: can't starting");
                    }
                }
                catch (Exception)
                {
                    Display.Message($"{file}: Error starting");
                }
            }
        }

        private static CsvWorkThread CreateFileHandlerThread()
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

        private static void CsvWT_WorkCompleted(object sender, bool aborted)
        {
            lock (locker)
            {
                var csvWT = sender as CsvWorkThread;
                if (csvWT != null)
                {
                    if (aborted) { Display.Message($"{csvWT.Name}: Processing aborted", ConsoleColor.Red); }
                    else { Display.Message($"{csvWT.Name}: Processing completed", ConsoleColor.Green); }

                    csvWT.WorkCompleted -= CsvWT_WorkCompleted;
                    csvWT.ErrorEvent -= CsvWT_ErrorEvent;

                    lstThread.Remove(csvWT);
                    Display.Message($"Number of file handler threads - {lstThread.Count}", ConsoleColor.Blue);
                }
            }

        }

        private static void CsvWT_ErrorEvent(object sender, string msg)
        {
            lock (locker)
            {
                Display.Message($"{(sender as CsvWorkThread)?.Name}: " + msg, ConsoleColor.Yellow);
            }
        }        

        #endregion // CSVWORKTHREAD_EVENTS



    }
}
