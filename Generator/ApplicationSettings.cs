using System;
using System.IO;

namespace Generator
{
    internal class ApplicationSettings
    {
        public const int GygabyteBytesCount = 1024 * 1024 * 1024;

        public byte Size { get; set; } = 1; // in GB
        public FileInfo Output { get; set; } = new FileInfo($"result_{DateTime.Now.ToString().Replace(':', '_')}.txt");

        public long GetSizeInBytes()
        {
            return Size * (long)GygabyteBytesCount;
        }
    }
}
