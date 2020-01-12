using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    class Program
    {
        private static readonly object locker = new object();

        static int Summ(object obj)
        {
            int n = (int)obj;
            //lock (locker) { 
                ++n;
                Console.WriteLine($"TaskId: {Task.CurrentId}  write {n}");
            //}
            return n;
        }


        static void Main(string[] args)
        {
            List<Thread> tasks = new List<Thread>();
            for (int i = 0; i < 100; i+=10)
            {
                //lock (locker)
                //{
                    Thread thread = new Thread(new ThreadStart(Summ));
                    tasks.Add(thread);
                //}
            }

            foreach (var task in tasks)
            {
                task.Start();
                Console.WriteLine($"TaskId: {Task.CurrentId}  write {task.Result}");
            }

            Console.ReadKey();
        }


    }
}
