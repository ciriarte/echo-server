using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin;
using Microsoft.Owin.Hosting;

using NDesk.Options;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace echo
{
    class Program
    {
        static void Main(string[] args)
        {
           bool showHelp = false;
           int port      = 9000;

           var p = new OptionSet () {
                { "p|port=", "the {PORT} for the echo server. Default: 9000.",
                  v => Int32.TryParse(v, out port) },
                { "h|help",  "show this message and exit", 
                  v => showHelp = v != null },
            };

           List<string> extra;
           try
           {
               extra = p.Parse(args);
           }
           catch (OptionException e)
           {
               Console.Write("echo: ");
               Console.WriteLine(e.Message);
               Console.WriteLine("Try `echo --help' for more information.");
               return;
           }

           if (showHelp)
           {
               ShowHelp(p);
               return;
           }

            string baseAddress = String.Format("http://localhost:{0}/", port);

            ConfigureLoggging();

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Running echo server on {0}", baseAddress);
                Console.ReadLine();
            }
        }

        static void ConfigureLoggging()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            consoleTarget.Layout = @"${date}|${logger}|${message}";

            var loggingRule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(loggingRule);

            LogManager.Configuration = config;
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: echo [OPTIONS]+ message");
            Console.WriteLine("Starts a small echo server for testing.");
            Console.WriteLine("If no port is specified, port 9000 will be used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
