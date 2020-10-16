using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Sorter
{
    class ChunkFile : IEnumerator<Record>
    {
        private readonly StreamReader _streamReader;
        private readonly FileInfo _file;

        private readonly Queue<Record> _cache = new Queue<Record>(1000);

        public ChunkFile(FileInfo file)
        {
            this._file = file;
            _streamReader = file.OpenText();

            Current = new Record(_streamReader.ReadLine());
            FillCache();
        }

        public Record Current { get; private set; }

        object IEnumerator.Current => Current;

        public FileInfo File => _file;

        public void Dispose()
        {
            _streamReader.Dispose();
            _file.Delete();
        }

        public bool MoveNext()
        {
            if (_cache.Count == 0)
            {
                FillCache();
                if (_cache.Count == 0 && _streamReader.EndOfStream)
                    return false;
            }

            var line = _cache.Dequeue();
            Current = line;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        private void FillCache()
        {
            for (int i = 0; i < 1000 && !_streamReader.EndOfStream; i++)
            {
                var line = _streamReader.ReadLine();
                _cache.Enqueue(new Record(line));
            }
        }
    }

}