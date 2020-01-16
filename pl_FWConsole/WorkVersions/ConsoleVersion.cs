using FolderWatcher.Versions;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace pl_FWConsole.WorkVersions
{
    internal static class ConsoleVersion
    {
        private static FSWVersion _fswatcher;

        internal static void Run()
        {
            string watchFolder = ConfigurationManager.AppSettings["DefaultWatchedFolder"];
            string watchFilter = ConfigurationManager.AppSettings["DefaultWatchedFilter"];

            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    watchFolder = dialog.SelectedPath;
            }

            _fswatcher = FSWVersion.CreateInstance(watchFolder, watchFilter);
            _fswatcher.NewFileDetectedEvent += FwLogger_NewFileDetectedEvent;
            Thread fwThread = new Thread(new ThreadStart(_fswatcher.Launch));
            fwThread.Start();
        }

        /// <summary>
        /// New file detected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="filePath"></param>
        private static void FwLogger_NewFileDetectedEvent(object sender, string filePath)
        {
            WorkingProcess.Start(filePath);
        }


        /// <summary>
        /// Stop file watcher
        /// </summary>
        internal static void StopFileWatcher()
        {
            _fswatcher.NewFileDetectedEvent -= FwLogger_NewFileDetectedEvent;
            _fswatcher.Stop();
        }
    }
}
