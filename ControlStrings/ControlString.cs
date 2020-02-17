namespace ControlStrings
{
    using System.Collections.Generic;
    using System.Linq;

    public class ControlString
    {
        public ControlString(int index, int length, Queue<string> values)
        {
            Index = index;
            Values = values ?? throw new System.ArgumentNullException(nameof(values));
            Length = length;
        }

        public int Index { get; set; }

        public int Length { get; set; }

        public ControlString NextControlString
        {
            get
            {
                var value = Values.Peek();

                return new ControlString(Index + value.Length, Length - value.Length, new Queue<string>(Values.Skip(1)));
            }
        }

        public Queue<string> Values { get; set; }
    }
}