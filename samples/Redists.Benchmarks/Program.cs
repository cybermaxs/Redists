using Redists.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redists.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Redists.Benchmarks");

            var options = ConfigurationOptions.Parse("localhost:6379");
            options.AbortOnConnectFail=false;
            options.AllowAdmin=true;
            var mux = ConnectionMultiplexer.Connect(options);
            if(!mux.IsConnected)
            {
                Console.WriteLine("Could not connnect to redis instance.");
                return;
            }

            var tsOptions=new TimeSeriesOptions(3600*1000, 1, true, TimeSpan.FromHours(2));
            var client = TimeSeriesFactory.New(mux.GetDatabase(0), "myts", tsOptions);
            var gen = new Random();

            var watcher = Stopwatch.StartNew();

            var tasks = new List<Task>();
            foreach(var i in Enumerable.Range(1, 5000))
            {
                tasks.Add(client.AddAsync(DateTime.UtcNow, gen.Next() ));
            }

            Task.WaitAll(tasks.ToArray());
            watcher.Stop();
            
            Console.WriteLine(string.Format("DONE in {0} ms", watcher.ElapsedMilliseconds));
            
#if DEBUG
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif

        }
    }
}
