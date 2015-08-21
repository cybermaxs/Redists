using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;

namespace Redists.RandomMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);

            Process.Start("http://localhost:8080/index.html");

            Console.ReadLine();
        }
    }
}
