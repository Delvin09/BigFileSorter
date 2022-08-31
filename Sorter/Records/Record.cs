using System;

namespace Sorter.Records
{
    readonly struct Record : IComparable<Record>
    {
        public const int Separator = '.';

        private readonly int _separatorPosition;

        private readonly ushort _index;

        private readonly RecordSpan _text;

        private readonly RecordSpan _input;

        public ReadOnlySpan<char> Span => _input.Span;

        public Record(RecordSpan input, int separatorPosition)
        {
            _input = input;
            _separatorPosition = separatorPosition;
            _index = ToUShort(input.Slice(0, _separatorPosition));
            _text = input.Slice(_separatorPosition + 2);
        }

        public int CompareTo(Record other)
        {
            var result = _text.CompareTo(other._text);
            if (result == 0)
                result = _index.CompareTo(other._index);
            return result;
        }

        private static ushort ToUShort(RecordSpan span)
        {
            int result = 0;
            for (int i = 0; i < span.Count; i++)
                result = result * 10 + (span[i] - '0');
            return (ushort)result;
        }
    }
}