using pl_FWConsole.Interfaces;
using pl_FWConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pl_FWConsole.ConsoleMenus
{
    internal static class Display
    {

        /// <summary>
        /// Wait push key 
        /// </summary>
        /// <param name="str"></param>
        internal static ConsoleKeyInfo WaitForContinue(string str = "", ConsoleColor color = ConsoleColor.Green)
        {
            if (!String.IsNullOrEmpty(str.Trim()))
            {
                Console.ForegroundColor = color;
                Console.WriteLine(str);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            return Console.ReadKey();
        }

        internal static void Message(string str, ConsoleColor color = ConsoleColor.White)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                Console.ForegroundColor = color;
                Console.WriteLine(str);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        /// <summary>
        /// Display information from DB
        /// </summary>
        /// <param name="repo"></param>
        internal static void DisplayData<T>(IEnumerable<T> entities, int count = 20) where T : IEntity
        {
            Message(typeof(T).ToString().ToUpper() + ":", ConsoleColor.Green);
            Console.WriteLine();

            int n = 0;
            foreach (var entity in entities)
            {
                Message($"{entity.Id}.{entity.Name}:", ConsoleColor.Yellow);
                foreach (SaleModel sale in entity.Sales)
                {
                    Console.WriteLine("- sale {0, 2}: {1, 2}  | {2, 3}. {3, -10} | {4, 3}. {5, -10} - {6, 3} | {7, 3}. {8, -10} | {9, 3}. {10, 3}"
                        , sale.Id
                        , sale.DTG.ToString("dd.MM.yyyy HH:mm")
                        , sale.Manager?.Id, sale.Manager?.Name
                        , sale.Product?.Id, sale.Product?.Name, sale.Product?.Cost
                        , sale.Client?.Id, sale.Client?.Name
                        , sale.FileName?.Id, sale.FileName?.Name
                        );
                    if (++n > count) { break; }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }



    }
}
