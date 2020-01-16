using pl_WinSeviceMenu.ConsoleMenus;
using System;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading;

namespace pl_WinSeviceMenu
{
    class Program
    {
        public delegate void method();
        private static string _servicePath = ConfigurationManager.AppSettings["ServicePath"]
                                           ?? @"E:\_projects\epam\epamT4v2\bll_WinServiceFW\bin\Debug\bll_WinServiceFW.exe";

        static void Main(string[] args)
        {
            // MENU
            string[] items = { "Install Service", "Run Service", "Stop service", "Uninstall service", "Exit" };
            method[] methods = new method[] { InstallService, RunService, StopService, UninstallService, Exit };
            Task4Menu menu = new Task4Menu(items);
            int menuResult;
            do
            {
                menuResult = menu.PrintMenu();
                Console.WriteLine();
                methods[menuResult]();
            } while (menuResult != items.Length - 1);
        }


        private static void InstallService()
        {
            if (System.IO.File.Exists(_servicePath))
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(new[] { _servicePath });
                    Console.Clear();
                    Console.WriteLine("Service is installed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to install the service. Set manually. " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Service not found");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void RunService()
        {
            using (ServiceController svcController = new ServiceController("FWService"))
            {
                try
                {
                    if (svcController == null) 
                    {
                        Console.WriteLine("Service not found");
                    }

                    if (svcController?.Status != ServiceControllerStatus.Running)
                    {
                        svcController?.Start();
                        WaitStatus(svcController, ServiceControllerStatus.Running);
                        Console.WriteLine("Service is running.");
                    }
                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to run the service. Run manually.");
                }
                svcController?.Close();
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void StopService()
        {
            using (ServiceController svcController = new ServiceController("FWService"))
            {
                if (svcController == null)
                {
                    Console.WriteLine("Service not found");
                }

                try
                {
                    if (svcController?.Status != ServiceControllerStatus.Stopped)
                    {
                        svcController?.Stop();
                        WaitStatus(svcController, ServiceControllerStatus.Stopped);
                        Console.WriteLine("Service is stopped.");
                    }                    
                }
                catch (Exception)
                {
                }
                svcController?.Close();
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void UninstallService()
        {
            if (System.IO.File.Exists(_servicePath))
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(new[] { "/u", _servicePath });
                    Console.WriteLine("Service is uninstalled.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to uninstall the service. Uninstall manually. " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Service not found");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void Exit()
        {
        }

        /// <summary>
        /// Wait for change service status
        /// </summary>
        /// <param name="svcController"></param>
        /// <param name="status"></param>
        private static void WaitStatus(ServiceController svcController, ServiceControllerStatus status)
        {
            while (svcController.Status != status)
            {
                Thread.Sleep(100);
                svcController.Refresh();
            }
        }
    }
}
