using System;
using System.IO;

namespace Sorter
{
    class ApplicationSettings
    {
        public const int DefaultBufferSize = 1024 * 1024 * 1024; // 1GB

        /// <summary>
        /// In bytes
        /// </summary>
        public int BufferSize { get; set; } = DefaultBufferSize / 10;

        public FileInfo InputFile { get; set; }

        public FileInfo OutputFile { get; set; } = new FileInfo("Result_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '_') + ".txt");

        public DirectoryInfo TempDirectory { get; set; } = new DirectoryInfo("Temp");
    }

}