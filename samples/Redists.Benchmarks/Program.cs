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
        const int Iterations= 100;
        static void Main()
        {
            Console.WriteLine("Redists.Benchmarks");

            //setup connexion multiplexer
            var options = ConfigurationOptions.Parse("localhost:6379");
            options.AbortOnConnectFail = false;
            options.AllowAdmin = true;
            var mux = ConnectionMultiplexer.Connect(options);
            if (!mux.IsConnected)
            {
                Console.WriteLine("Could not connnect to redis instance.");
                return;
            }

            //setup redists  
            var db = mux.GetDatabase(0); 
            var tsOptions = new TimeSeriesOptions(3600 * 1000, 1, TimeSpan.FromDays(1));

            mux.GetServer("localhost:6379").FlushAllDatabases();
            Profile("OneByOne", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(db, "msts", tsOptions);
                Methods.AddAsync(client).Wait();
            });
            mux.GetServer("localhost:6379").FlushAllDatabases();
            Profile("Add100", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(db, "myts", tsOptions);
                Methods.Add100Async(client).Wait();
            });
            mux.GetServer("localhost:6379").FlushAllDatabases();
            Profile("Batch", Iterations, () =>
            {
                var client = TimeSeriesFactory.New(db, "msts", tsOptions);
                Methods.AddBatchof100Async(client).Wait();
            });

            mux.GetServer("localhost:6379").FlushAllDatabases();
            var tmpclient = TimeSeriesFactory.New(db, "myts", tsOptions);
            tmpclient.AddAsync(Enumerable.Range(1, 3600 * 24).Select(i => new DataPoint(DateTime.UtcNow.Date.AddSeconds(i), i)).ToArray());
            Profile("ReadAll dynamic", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(db, "myts3", tsOptions);
                Methods.ReadAllAsync(client).Wait();
            });
            var tmpclientfx = TimeSeriesFactory.NewFixed(db, "myts", tsOptions);
            tmpclientfx.AddAsync(Enumerable.Range(1, 3600 * 24).Select(i => new DataPoint(DateTime.UtcNow.Date.AddSeconds(i), i)).ToArray());
            Profile("ReadAll fixed", Iterations, () =>
            {
                var client = TimeSeriesFactory.NewFixed(mux.GetDatabase(0), "myts3", tsOptions);
                Methods.ReadAllAsync(client).Wait();
            });

            mux.Close(false);


#if DEBUG
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif

        }

        static void Profile(string description, int iterations, Action act)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            act();

             var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                act();
            }
            watch.Stop();

            Console.WriteLine($"{description} => {watch.ElapsedMilliseconds:0.00} ms ({watch.ElapsedTicks:N0} ticks)");
        }
    }
}
