using FluentScheduler;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Redists.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Web.Http;

namespace Redists.RandomMonitor
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            //redis
            RedisServer.Start();
            RedisServer.Reset();

            //create fake data for the last minute
            var tmpRandom = new Random();
            var tmpclient = TimeSeriesFactory.New(RedisServer.GetDatabase(0), "fkts", new TimeSeriesOptions(60 * 1000, 1000, TimeSpan.FromHours(1)));
            var start = DateTime.UtcNow.AddSeconds(-60);
            tmpclient.AddAsync(Enumerable.Range(1, 60).Select(t => new DataPoint(start.AddSeconds(t), tmpRandom.Next(1000))).ToArray()).Wait();
            //fake data generator
            TaskManager.AddTask(() =>
            {
                var client = TimeSeriesFactory.New(RedisServer.GetDatabase(0), "fkts", new TimeSeriesOptions(60 * 1000, 1000, TimeSpan.FromHours(1)));
                var r = new Random();
                client.AddAsync(DateTime.UtcNow, r.Next(1000)).Wait();
            }, s => { s.ToRunEvery(1).Seconds(); });

            // Configure Web API for self-host.          
            var apiConfig = new HttpConfiguration();
            apiConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(apiConfig);

            var fileSystem = new PhysicalFileSystem(".");
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem
            };

            appBuilder.UseFileServer(options);
        }
    }
}
