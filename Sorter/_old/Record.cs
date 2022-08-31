using System;

namespace Old.Sorter
{
    internal readonly struct Record : IComparable<Record>
    {
        private readonly string _rawLine;
        private readonly int _separatorPosition;
        private readonly ReadOnlyMemory<char> _text;

        public Record(string text)
        {
            _rawLine = text;

            _separatorPosition = FindSeparatorPos(text);
            Index = int.Parse(text.AsSpan(0, _separatorPosition));
            _text = _rawLine.AsMemory(_separatorPosition + 2);
        }

        public int Index { get; }

        public ReadOnlyMemory<char> Text { get => _text; }

        public int CompareTo(Record other)
        {
            var result = Text.Span.CompareTo(other.Text.Span, StringComparison.Ordinal);
            if (result == 0)
                return Index.CompareTo(other.Index);
            return result;
        }

        public override string ToString() => _rawLine;

        private static int FindSeparatorPos(string text)
        {
            int start, end, current;
            current = text.Length / 2;
            start = 0;
            end = text.Length - 1;

            if (text.Length > 5)
            {
                if (text[5] == '.')
                    return 5;
                if (text[5] == ' ' && text[4] == '.')
                    return 4;

                if (char.IsLetter(text[5]))
                    end = 4;
                else if (char.IsDigit(text[5]))
                    start = 5;
            }

            while (start < end)
            {
                var currentChar = text[current];

                if (currentChar == '.')
                    return current;

                if (currentChar == ' ' && text[current - 1] == '.')
                    return current - 1;

                if (char.IsDigit(currentChar))
                {
                    start = current;
                    current = (start + end) / 2;
                }
                else
                {
                    end = current;
                    current = (start + end) / 2;

                }
            }
            return -1;
        }
    }
}
