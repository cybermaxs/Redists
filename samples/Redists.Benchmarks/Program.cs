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
            var tsOptions = new TimeSeriesOptions(3600 * 1000, 1, TimeSpan.FromDays(1));
            
            Profile("OneByOne", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(mux.GetDatabase(0), "myts1", tsOptions);
                Methods.AddAsync(client).Wait();
            });
            Profile("Add100", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(mux.GetDatabase(0), "myts2", tsOptions);
                Methods.Add100Async(client).Wait();
            });
            Profile("Batch", Iterations, () =>
            {
                var client = TimeSeriesFactory.New(mux.GetDatabase(0), "myts2", tsOptions);
                Methods.AddBatchof100Async(client).Wait();
            });
            Profile("ReadAll", Iterations,() =>
            {
                var client = TimeSeriesFactory.New(mux.GetDatabase(0), "myts3", tsOptions);
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
