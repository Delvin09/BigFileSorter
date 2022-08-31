using Sorter.Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Sorter.Chunks
{
    internal class ChunkFile : IEnumerator<Record>
    {
        private readonly LineReader _reader;
        private readonly FileInfo _file;

        private IEnumerator<Record> _iterator;

        public ChunkFile(FileInfo file, int bufferSize)
        {
            _file = file;
            _reader = new LineReader(file.OpenText(), bufferSize);
            _iterator = _reader.ReadNext().GetEnumerator();
            _iterator.MoveNext();
        }

        public Record Current => _iterator.Current;

        object IEnumerator.Current => Current;

        public FileInfo File => _file;

        public void Dispose()
        {
            _reader.Dispose();
            _file.Delete();
        }

        public bool MoveNext()
        {
            var result = _iterator.MoveNext();
            if (!result)
            {
                _iterator = _reader.ReadNext().GetEnumerator();
                result = _iterator.MoveNext();
            }

            return result;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}