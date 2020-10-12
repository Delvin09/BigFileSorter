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

        public ChunkFile(FileInfo file)
        {
            this._file = file;
            _streamReader = file.OpenText();
            Current = new Record(_streamReader.ReadLine());
        }

        public Record Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _streamReader.Dispose();
            _file.Delete();
        }

        public bool MoveNext()
        {
            var line = _streamReader.ReadLine();
            if (line == null)
            {
                Current = null;
                return false;
            }
            Current = new Record(line);
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        internal void Delete()
        {
            throw new NotImplementedException();
        }
    }

}