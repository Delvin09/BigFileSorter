using System;

namespace Sorter
{
    struct Record : IComparable<Record>
    {
        public Record(string text)
        {
            var dot = text.IndexOf('.');
            Index = int.Parse(text.Substring(0, dot));
            Text = text.Substring(dot + 2);
        }

        public int Index { get; set; }

        public string Text { get; set; }

        public int CompareTo(Record other)
        {
            var result = string.CompareOrdinal(this.Text, other.Text);
            if (result == 0)
                return this.Index.CompareTo(other.Index);
            return result;
        }

        public override string ToString() => $"{Index}. {Text}";
    }

}