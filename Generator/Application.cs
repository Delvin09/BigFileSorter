using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generator
{
    internal static class Application
    {
        public static void Run(ApplicationSettings settings)
        {
            var processorCount = Environment.ProcessorCount;
            var file = settings.Output;
            using (var stream = file.Create())
            {
                while (stream.Length < settings.GetSizeInBytes())
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Generate in progress {stream.Length} of {settings.GetSizeInBytes()}");

                    var bufferSize = ApplicationSettings.GigabyteBytesCount / processorCount;
                    var appendix = ApplicationSettings.GigabyteBytesCount % processorCount;

                    var generators = new Generator[processorCount];
                    for (int i = 0; i < processorCount; i++)
                    {
                        generators[i] = new Generator(i == processorCount - 1 ? bufferSize + appendix : bufferSize);
                    }

                    Parallel.Invoke(generators.Select(g => new Action(g.Process)).ToArray());

                    foreach (var item in generators)
                    {
                        item.CopyTo(stream);
                        item.Dispose();
                    }

                    stream.Flush();
                }
            }
            Console.WriteLine();
        }
    }
}