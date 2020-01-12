namespace BLL.Interfaces
{
    interface ILaunchable
    {
        bool IsLaunched { get; }

        void Launch();
        void Stop();

    }
}
