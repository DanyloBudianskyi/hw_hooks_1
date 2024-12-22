using System;
using System.IO;
using EasyHook;

namespace Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            int targetPID = 0;

            Console.Write("Enter target process ID: ");
            targetPID = Int32.Parse(Console.ReadLine());

            string channelName = null;

            RemoteHooking.IpcCreateServer<SleepHook.ServerInterface>(
                ref channelName,
                System.Runtime.Remoting.WellKnownObjectMode.Singleton);

            string injectionLibrary = (@"C:\Users\Danil\Desktop\Dll_hook\Dll_hook\bin\Debug\Dll_hook.dll");

            try
            {
                RemoteHooking.Inject(
                    targetPID,
                    injectionLibrary,
                    injectionLibrary,
                    channelName);

                Console.WriteLine("Injection successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Injection failed: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
