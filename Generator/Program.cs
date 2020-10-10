using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var settings = GetApplicationSettings(args);
            Application.Run(settings);

            stopwatch.Stop();
            Console.WriteLine("Done! Time: " + stopwatch.Elapsed);
        }

        private static ApplicationSettings GetApplicationSettings(string[] args)
        {
            var commandLineParser = new CommandLineApplication();
            var settings = new ApplicationSettings();
            var sizeOption = commandLineParser.Option<byte>("-s|--size <BYTE>", "Size of ouptput file in Gigabyte.", CommandOptionType.SingleValue);
            var outputOption = commandLineParser.Option<string>("-o|--output <STRING>", "Output file path.", CommandOptionType.SingleValue);

            commandLineParser.HelpOption();

            commandLineParser.OnExecute(() =>
            {
                settings.Size = sizeOption.HasValue() && sizeOption.ParsedValue > 0 ? sizeOption.ParsedValue : (byte)1;
                settings.Output = new FileInfo(outputOption.HasValue() ? outputOption.ParsedValue : $"result_{DateTime.Now}.txt");
            });

            commandLineParser.Execute(args);
            return settings;
        }
    }
}
