using System;
using System.IO;

namespace Sorter
{
    class ApplicationSettings
    {
        public const int DefaultBufferSize = 12000000;

        public int BufferSize { get; set; } = DefaultBufferSize;

        public FileInfo InputFile { get; set; }

        public FileInfo OutputFile { get; set; } = new FileInfo("Result_" + DateTime.Now.ToString().Replace(':', '_') + ".txt");

        public DirectoryInfo TempDirectory { get; set; } = new DirectoryInfo("Temp");
    }

}