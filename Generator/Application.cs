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
            // TODO: need to add the calculation of the buffer size based on the size of free|total memory.
            // TODO: need to add a setting to use parallelism (parallel calculation or disable it).
            var processorCount = Environment.ProcessorCount;
            var bufferSize = ApplicationSettings.GigabyteBytesCount / processorCount;
            var appendix = ApplicationSettings.GigabyteBytesCount % processorCount;

            var generators = new Generator[processorCount];
            for (int i = 0; i < processorCount; i++)
            {
                generators[i] = new Generator(i == processorCount - 1 ? bufferSize + appendix : bufferSize);
            }

            var file = settings.Output;
            using (var stream = file.Create())
            {
                while (stream.Length < settings.GetSizeInBytes())
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Generate in progress {stream.Length} of {settings.GetSizeInBytes()}");

                    Parallel.Invoke(generators.Select(g => new Action(g.Process)).ToArray());

                    foreach (var item in generators)
                    {
                        item.CopyTo(stream);
                    }

                    stream.Flush();
                }

                foreach (var generator in generators)
                {
                    generator.Dispose();
                }
            }
            Console.WriteLine();
        }
    }
}