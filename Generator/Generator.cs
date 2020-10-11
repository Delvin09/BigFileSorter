using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace Generator
{
    // TODO: add progress
    internal class Generator : IDisposable
    {
        private readonly Random _random = new Random();
        private static readonly string[] _dic = { "object", "hospitable", "harass", "salty", "quickest", "school", "courageous", "spiritual", "grandfather", "miss", "time", "profit" };
        private readonly int _bufferSize;
        private readonly MemoryStream _buffer;

        public Generator(int bufferSize)
        {
            _bufferSize = bufferSize;
            _buffer = new MemoryStream(_bufferSize);
        }

        public void Process()
        {
            var writer = new StreamWriter(_buffer);
            _buffer.Position = 0;

            while (_buffer.Length < _bufferSize)
            {
                writer.WriteLine(GenRow());
            }

            writer.Flush();
        }

        public void CopyTo(Stream stream)
        {
            _buffer.Position = 0;
            _buffer.CopyTo(stream);
        }

        public void Dispose()
        {
            _buffer.Close();
        }

        private string GenRow()
        {
            var count = _random.Next(1, _dic.Length + 1);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(_random.Next(1, ushort.MaxValue)).Append(". ");
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    stringBuilder.Append(" ");
                stringBuilder.Append(_dic[_random.Next(0, _dic.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
