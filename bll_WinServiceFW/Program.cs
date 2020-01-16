using System.ServiceProcess;
using System.Windows.Forms;

namespace bll_WinServiceFW
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FWService()
            };
            try
            {
                ServiceBase.Run(ServicesToRun);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
