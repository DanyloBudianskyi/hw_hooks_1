using System;
using System.Threading;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Sleep for 3s...");
                Thread.Sleep(3000);
            }
        }
    }
}
