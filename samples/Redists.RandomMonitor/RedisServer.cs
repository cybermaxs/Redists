using StackExchange.Redis;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Redists.RandomMonitor
{
    public class RedisServer
    {
        private static ConnectionMultiplexer mux;

        private static bool wasStarted = false;
        private const string connectionString = "localhost:6379,allowAdmin=true";

        public static bool Start()
        {
            if (!IsRunning)
            {
                Process.Start(@"..\..\..\..\packages\Redis-64.2.8.21\redis-server.exe");
                wasStarted = true;
            }
            Thread.Sleep(1000);
            mux = ConnectionMultiplexer.Connect("localhost:6379,allowAdmin=true");

            return wasStarted && mux != null;
        }

        public static void Close()
        {
            if (mux != null && mux.IsConnected)
                mux.Close(false);

            if (wasStarted)
            {
                foreach (var proc in Process.GetProcessesByName("redis-server"))
                    proc.Kill();
            }
        }

        public static IDatabase GetDatabase(int db)
        {
            return mux.GetDatabase(db);
        }

        public static bool IsRunning
        {
            get
            {
                return Process.GetProcessesByName("redis-server").Count() > 0;
            }
        }

        public static void Reset()
        {
            mux.GetServer("localhost:6379").FlushAllDatabases();
        }

        public static void Kill()
        {
            foreach (var p in Process.GetProcessesByName(@"redis-server"))
            {
                p.Kill();
            }
        }
    }
}
