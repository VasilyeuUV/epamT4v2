namespace FolderWatcher.Interfaces
{
    interface ILauncable
    {
        bool IsLaunched { get; }

        void Launch();
        void Stop();
    }
}
