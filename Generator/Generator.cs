using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace Generator
{
    // TODO: Multithreading, Add dictionary, add progress
    internal class Generator
    {
        private static readonly Random _random = new Random();
        private static readonly string[] _dic = { "test", "test2", "test3", "test4", "test4", "test5" };

        private string GenRow(long index = 1)
        {
            var count = _random.Next(1, _dic.Length + 1);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(index).Append(". ");
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    stringBuilder.Append(" ");
                stringBuilder.Append(_dic[_random.Next(0, _dic.Length)]);
            }
            return stringBuilder.ToString();
        }

        private void Proc(ConcurrentBag<StringBuilder> bag, long limit)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (long i = 0; i <= limit;)
            {
                var row = GenRow(1);
                i += row.Length;
                stringBuilder.AppendLine(row);
            }
            bag.Add(stringBuilder);
        }

        private void Proc(Stream stream, ApplicationSettings settings)
        {
            while (stream.Length < settings.GetSizeInBytes())
            {
                using (var buffer = new MemoryStream(ApplicationSettings.GygabyteBytesCount))
                using (var bufferWriter = new StreamWriter(buffer))
                {
                    buffer.Position = 0;

                    while (buffer.Length < ApplicationSettings.GygabyteBytesCount)
                    {
                        bufferWriter.WriteLine(GenRow());
                    }

                    bufferWriter.Flush();
                    buffer.Position = 0;
                    buffer.CopyTo(stream);
                }
                stream.Flush();
            }
        }

        public void Process(ApplicationSettings settings)
        {
            //var treadCount = Environment.ProcessorCount;
            //var treads = new Task[treadCount];
            //var bag = new ConcurrentBag<StringBuilder>();
            //for (int i = 0; i < treadCount; i++)
            //{
            //    treads[i] = new Task(() => Proc(bag, settings.GetSizeInBytes() / treadCount));
            //}

            //foreach (var t in treads)
            //    t.Start();

            //Task.WaitAll(treads);

            //var file = settings.Output;
            //using (var stream = file.CreateText())
            //{

            //    foreach (var item in bag)
            //    {
            //        stream.WriteLine(item.ToString());
            //    }
            //    stream.Flush();
            //}

            var file = settings.Output;
            using (var streamWriter = file.CreateText())
            {
                Proc(streamWriter.BaseStream, settings);
            }
        }
    }
}
