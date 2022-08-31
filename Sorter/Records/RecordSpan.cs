using System;

namespace Sorter.Records
{
    readonly struct RecordSpan : IComparable<RecordSpan>
    {
        private readonly char[] _inner;
        private readonly Range _range;

        public RecordSpan(char[] inner, Range range)
        {
            _inner = inner;
            _range = range;
        }

        public ReadOnlySpan<char> Span => _inner.AsSpan(_range);

        public char this[int index] => _inner[_range.GetOffsetAndLength(_inner.Length).Offset + index];

        public int Count => _range.GetOffsetAndLength(_inner.Length).Length;

        public int CompareTo(RecordSpan other)
        {
            int lengthA = _range.GetOffsetAndLength(_inner.Length).Length;
            int lengthB = other._range.GetOffsetAndLength(_inner.Length).Length;
            int length = Math.Min(lengthA, lengthB);

            int indexA = _range.Start.Value;
            int indexB = other._range.Start.Value;
            int charA = _inner[indexA];
            int charB = other._inner[indexB];
            while (length != 0)
            {
                if (charA != charB)
                    return charA - charB;

                charA = _inner[indexA];
                charB = other._inner[indexB];
                indexA++; indexB++;
                length--;
            }

            return lengthA - lengthB;
        }

        public RecordSpan Slice(int startIndex)
        {
            return new RecordSpan(_inner, new Range(_range.Start.Value + startIndex, _range.End));
        }

        public RecordSpan Slice(int startIndex, int endIndex)
        {
            return new RecordSpan(_inner, new Range(_range.Start.Value + startIndex, _range.Start.Value + endIndex));
        }
    }
}