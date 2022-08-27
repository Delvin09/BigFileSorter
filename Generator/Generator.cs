using System;
using System.IO;
using System.Text;

namespace Generator
{
    internal class Generator : IDisposable
    {
        private static readonly string[] _dic = { "object", "hospitable", "harass", "salty", "quickest", "school", "courageous", "spiritual", "grandfather", "miss", "time", "profit" };
        private static int _randomCount = 1;

        private readonly Random _random = new Random(_randomCount++);
        private readonly int _bufferSize;
        private MemoryStream _buffer;
        private string[] _wordBuffer = new string[_dic.Length];

        public Generator(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public void Process()
        {
            _buffer?.Dispose();
            _buffer = new MemoryStream(_bufferSize);

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

        private StringBuilder GenRow()
        {
            var count = _random.Next(1, _dic.Length + 1);
            var number = _random.Next(1, ushort.MaxValue);
            int totalLength = 0;
            for (int i = 0; i < count; i++)
            {
                var str = _dic[_random.Next(0, _dic.Length)];
                _wordBuffer[i] = str;
                totalLength += str.Length;
            }

            StringBuilder stringBuilder = new StringBuilder(totalLength + count + 7);
            stringBuilder.Append(number).Append(". ").Append(_wordBuffer[0]);

            for (int i = 1; i < count; i++)
            {
                stringBuilder.Append(" ").Append(_wordBuffer[i]);
            }
            return stringBuilder;
        }
    }
}
