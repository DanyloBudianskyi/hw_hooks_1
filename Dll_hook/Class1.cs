using System;
using System.Runtime.InteropServices;
using EasyHook;

namespace Dll_hook
{
    public class Hook : IEntryPoint
    {
        private readonly ServerInterface _server;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SleepDelegate(int dwMilliseconds);
        private static SleepDelegate _originalSleep;

        public static void HookedSleep(int dwMilliseconds)
        {
            Console.WriteLine($"Sleep called with {dwMilliseconds} ms");
            _originalSleep(100);
        }

        public Hook(RemoteHooking.IContext context, string channelName)
        {
            _server = RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
            _server.Ping();
        }

        public void Run(RemoteHooking.IContext context, string channelName)
        {
            try
            {
                var hook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "Sleep"),
                    new SleepDelegate(HookedSleep),
                    this
                );

                _originalSleep = Marshal.GetDelegateForFunctionPointer<SleepDelegate>(
                    LocalHook.GetProcAddress("kernel32.dll", "Sleep"));

                hook.ThreadACL.SetExclusiveACL(new[] { 0 });

                _server.ReportMessage("Hook installed.");

                while (true)
                {
                    Console.WriteLine("Hook is working");
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                _server.ReportMessage($"Error: {ex.Message}");
            }
        }
    }

    public class ServerInterface : MarshalByRefObject
    {
        public void Ping() { }

        public void ReportMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
