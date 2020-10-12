using System;

namespace Sorter
{
    class Record : IComparable<Record>
    {
        public Record(string text)
        {
            var segments = text.Split('.');
            Index = int.Parse(segments[0]);
            Text = segments[1].TrimStart();
        }

        public int Index { get; set; }

        public string Text { get; set; }

        public int CompareTo(Record other)
        {
            var result = StringComparer.Ordinal.Compare(this.Text, other.Text);
            if (result == 0)
                return this.Index.CompareTo(other.Index);
            return result;
        }

        public override string ToString() => $"{Index}. {Text}";
    }

}