namespace ControlStrings
{
    using System.Collections.Generic;
    using System.Linq;

    public class ControlString
    {
        public int Index { get; set; }

        public Queue<string> Values { get; set; }

        public ControlString(int index, int length, Queue<string> values)
        {
            Index = index;
            Values = values;
            Length = length;
        }

        public ControlString NextControlString
        {
            get
            {
                return new ControlString(Index + Values.Peek().Length, Length - Values.Peek().Length, new Queue<string>(Values.Skip(1)));
            }
        }

        public int Length { get; set; }
    }
}