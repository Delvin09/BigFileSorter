using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var settings = GetApplicationSettings(args);
            if (settings != null)
                Application.Run(settings);
            stopwatch.Stop();
            Console.WriteLine($"Total time: {stopwatch.Elapsed}");
        }

        private static ApplicationSettings GetApplicationSettings(string[] args)
        {
            var commandLineParser = new CommandLineApplication();
            var settings = new ApplicationSettings();

            var inputOption = commandLineParser.Option<string>("-i|--input <STRING>", "Input file path.", CommandOptionType.SingleValue)
                .IsRequired();

            var outputOption = commandLineParser.Option<string>("-o|--output <STRING>", "Output file path.", CommandOptionType.SingleValue);
            var tempOption = commandLineParser.Option<string>("-t|--temp <STRING>", "Temp directory path.", CommandOptionType.SingleValue);
            var bufferOption = commandLineParser.Option<int>("-b|--buffer <BYTE>", $"Buffer size. By defult: {ApplicationSettings.DefaultBufferSize}.", CommandOptionType.SingleValue);

            var helpOption = commandLineParser.HelpOption();

            commandLineParser.OnExecute(() =>
            {
                settings.InputFile = new FileInfo(inputOption.ParsedValue);

                if (outputOption.HasValue())
                    settings.OutputFile = new FileInfo(outputOption.ParsedValue);
                if (tempOption.HasValue())
                    settings.TempDirectory = new DirectoryInfo(tempOption.ParsedValue);
                if (bufferOption.HasValue())
                    settings.BufferSize = bufferOption.ParsedValue;
            });

            commandLineParser.Execute(args);
            return helpOption.HasValue() || !inputOption.HasValue() ? null : settings;
        }
    }

}