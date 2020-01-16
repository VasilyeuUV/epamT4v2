using FolderWatcher.Versions;
using pl_FWConsole.ConsoleMenus;
using pl_FWConsole.WorkVersions;
using System;
using System.Linq;

namespace pl_FWConsole
{
    class Program
    {
        public delegate void method();

        [STAThread]
        static void Main(string[] args)
        {
            // MENU
            string[] items = { "Create file", "Start folder monitoring", "View Data", "Exit" };
            method[] methods = new method[] { RunOpenFileVertion, RunConsoleVersion, DisplayData, Exit };
            Task4Menu menu = new Task4Menu(items);
            int menuResult;
            do
            {
                menuResult = menu.PrintMenu();
                Console.WriteLine();
                methods[menuResult]();
            } while (menuResult != items.Length - 1);
        }


        /// <summary>
        /// Start OpenFileVertion
        /// </summary>
        private static void RunOpenFileVertion()
        {
            Console.Clear();
            Display.Message($"OPEN_FILE_VERTION WORK", ConsoleColor.Green);

            if (FolderWatcherBase.IsLaunched)
            {
                Display.Message("Can't start FWService. FolderWather is launched.", ConsoleColor.Red);
                Display.WaitForContinue();
                return;
            }

            string[] files = OpenFileVersion.Run();

            if (files == null || files.Length < 1)
            {
                Display.WaitForContinue("Error opening one/several files", ConsoleColor.Red);
                return;
            }
            foreach (var file in files) { WorkingProcess.Start(file); }

            do
            {
            } while (WorkingProcess.lstThread.Count() > 0);
            Display.WaitForContinue();
        }

        /// <summary>
        /// Start Console Vertion
        /// </summary>
        private static void RunConsoleVersion()
        {
            Console.Clear();
            Display.Message($"CONSOLE_VERTION WORK", ConsoleColor.Green);

            ConsoleVersion.Run();

            Display.Message("Run the CSV file generator in Emulator");
            Console.WriteLine("");
            do
            {
                while (!Console.KeyAvailable)
                {
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            ConsoleVersion.StopFileWatcher();

            Display.WaitForContinue("File watcher is stopped", ConsoleColor.Green);
        }
        

        /// <summary>
        /// Display data
        /// </summary>
        private static void DisplayData()
        {
            Console.Clear();
            int viewPositions = 1000;

            //Display.DisplayData<ManagerModel>(viewPositions);
            //Display.DisplayData<FileNameModel>(viewPositions);
            //Display.DisplayData<ClientModel>(viewPositions);
            //Display.DisplayData<ProductModel>(viewPositions);

            Display.WaitForContinue();
        }

        /// <summary>
        /// Exit
        /// </summary>
        private static void Exit()
        {
            //Console.WriteLine("Press any key to Exit");
            //Console.ReadKey();
        }

    }
}
