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
            var settings = GetApplicationSettings(args);
            if (settings != null)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                 Application.Run(settings);

                stopwatch.Stop();
                Console.WriteLine("Done! Time: " + stopwatch.Elapsed);
            }
        }

        private static ApplicationSettings GetApplicationSettings(string[] args)
        {
            var commandLineParser = new CommandLineApplication();
            var settings = new ApplicationSettings();
            var sizeOption = commandLineParser.Option<byte>("-s|--size <BYTE>", "Size of ouptput file in Gigabyte.", CommandOptionType.SingleValue);
            var outputOption = commandLineParser.Option<string>("-o|--output <STRING>", "Output file path.", CommandOptionType.SingleValue);

            var helpOption = commandLineParser.HelpOption();

            commandLineParser.OnExecute(() =>
            {
                if (sizeOption.HasValue() && sizeOption.ParsedValue > 0)
                    settings.Size = sizeOption.ParsedValue;

                if (outputOption.HasValue())
                    settings.Output = new FileInfo(outputOption.ParsedValue);
            });

            commandLineParser.Execute(args);
            return helpOption.HasValue() ? null : settings;
        }
    }
}
