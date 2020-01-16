using BLL.Threads;
using bll_WinServiceFW.Infrastructure;
using FolderWatcher.Versions;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace bll_WinServiceFW
{
    public partial class FWService : ServiceBase
    {
        private Logger _logger;
        private FSWVersion _fswatcher;
        private CsvWorkThreadsControl _csvThreadControl;

        public FWService()
        {
            InitializeComponent();

            this.CanStop = true;                    // service can be stopped
            this.CanPauseAndContinue = true;        // service can be paused and then continued
            this.AutoLog = true;                    // service can record to the log  
        }

        protected override void OnStart(string[] args)
        {
            if (FolderWatcherBase.IsLaunched)
            {
                throw new System.Exception("Can't start FWService. FolderWather is launched.");
            }


            _logger = new Logger(ConfigurationManager.AppSettings["LogFile"]);

            string watchFolder = ConfigurationManager.AppSettings["DefaultWatchedFolder"] ?? @"D:\epam_temp\";
            string watchFilter = ConfigurationManager.AppSettings["DefaultWatchedFilter"] ?? @"*.csv"; ;

            _logger.RecordEvent($"Folder: {watchFolder}");
            _logger.RecordEvent($"Filter: {watchFilter}");

            _csvThreadControl = new CsvWorkThreadsControl();
            _csvThreadControl.SendMessage += _csvThreadControl_SendMessage;
            _logger.RecordEvent($"CSV thread controller started");

            _fswatcher = FSWVersion.CreateInstance(watchFolder, watchFilter);
            _logger.RecordEvent($"Folder Watcher created");
            _fswatcher.NewFileDetectedEvent += _fswatcher_NewFileDetectedEvent;
            Thread fwThread = new Thread(new ThreadStart(_fswatcher.Launch));
            fwThread.Start();
            _logger.RecordEvent($"Service starting. Watching folder: {watchFolder}");

        }

        protected override void OnStop()
        {
            _csvThreadControl.SendMessage -= _csvThreadControl_SendMessage;
            _csvThreadControl = null;
            _logger.RecordEvent($"CSV thread controller stopped");

            _fswatcher.Stop();
            _fswatcher.NewFileDetectedEvent -= _fswatcher_NewFileDetectedEvent;
            _fswatcher.Dispose();
            _fswatcher = null;
            _logger.RecordEvent($"FWService stopped.");

            _logger = null;

            Thread.Sleep(1000);
        }


        #region EVENT_HANDLERS


        private void _csvThreadControl_SendMessage(object sender, string msg)
        {
            _logger.RecordEvent(msg);
        }

        private void _fswatcher_NewFileDetectedEvent(object sender, string filePath)
        {
            _csvThreadControl.Start(filePath);
        }

        #endregion // EVENT_HANDLERS


    }
}
