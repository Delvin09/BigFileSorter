using Sorter.Records;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sorter.Chunks
{
    class LineReader : IDisposable
    {
        private int _bufferSize;
        private readonly char[] _buffer;

        private int _bufferPosition = -1;

        public LineReader(StreamReader streamReader, int bufferSize)
        {
            StreamReader = streamReader;
            _bufferSize = bufferSize;
            _buffer = new char[bufferSize];
            _bufferPosition = 0;
        }

        public StreamReader StreamReader { get; }

        public bool IsEOF => StreamReader.EndOfStream;

        public void Dispose()
        {
            StreamReader.Dispose();
        }

        public IEnumerable<Record> ReadNext()
        {
            if (_bufferPosition <= 0)
            {
                _bufferSize = StreamReader.ReadBlock(_buffer, 0, _buffer.Length);
                _bufferPosition = 0;
            }
            else
            {
                Array.ConstrainedCopy(_buffer, _bufferPosition, _buffer, 0, _buffer.Length - _bufferPosition);
                _bufferSize = StreamReader.ReadBlock(_buffer, _buffer.Length - _bufferPosition, _buffer.Length - (_buffer.Length - _bufferPosition));
                if (_bufferSize > 0 && _bufferPosition > 0 && _bufferPosition < _buffer.Length) _bufferSize += _buffer.Length - _bufferPosition;
                _bufferPosition = 0;
            }
            return GetLines((line, pos) => new Record(line, pos), Record.Separator);
        }

        private IEnumerable<T> GetLines<T>(Func<RecordSpan, int, T> selector, int separator)
        {
            int separatorPos = -1;
            for (int i = _bufferPosition; i < _bufferSize; i++)
            {
                int data = _buffer[i];
                if (data == -1 || data == 0) // end of file
                {
                    if (i > _bufferPosition)
                    {
                        var mem = new RecordSpan(_buffer, new Range(_bufferPosition, i));
                        separatorPos -= _bufferPosition;
                        _bufferPosition = _buffer.Length;
                        yield return selector(mem, separatorPos);
                    }

                    if (!StreamReader.EndOfStream) throw new InvalidOperationException();
                    yield break;
                }

                if (data == 13) // if a caret character was found
                {
                    if (i + 1 >= _buffer.Length) // but it was last char in the buffer array
                    {
                        var pos = StreamReader.BaseStream.Position;
                        var nextChar = StreamReader.Read();
                        if (nextChar == 10) // if next char is a new line than return item, if not - restore position in the stream
                        {
                            var mem = new RecordSpan(_buffer, new Range(_bufferPosition, i));
                            separatorPos -= _bufferPosition;
                            _bufferPosition = _buffer.Length;
                            yield return selector(mem, separatorPos);
                        }
                        else
                            StreamReader.BaseStream.Position = pos;
                    }
                    else if (_buffer[i + 1] == 10)
                    {
                        var mem = new RecordSpan(_buffer, new Range(_bufferPosition, i));
                        separatorPos -= _bufferPosition;
                        i += 2;
                        _bufferPosition = i;
                        yield return selector(mem, separatorPos);
                    }
                    //TODO: if a caret symbol only one?
                }

                if (data == 10) // if a new line char only one
                {
                    var mem = new RecordSpan(_buffer, new Range(_bufferPosition, i));
                    separatorPos -= _bufferPosition;
                    _bufferPosition = i + 1;
                    yield return selector(mem, separatorPos);
                }

                if (data == separator)
                    separatorPos = i;
            }
        }
    }
}

