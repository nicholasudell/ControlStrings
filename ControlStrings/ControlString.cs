namespace ControlStrings
{
    using System.Collections.Generic;
    using System.Linq;

    public class ControlString
    {
        public ControlString(int index, int length, Queue<string> values, Queue<string> transformers)
        {
            Index = index;
            Values = values ?? throw new System.ArgumentNullException(nameof(values));
            Transformers = transformers;
            Length = length;
        }

        public int Index { get; }

        public int Length { get; }

        public ControlString NextControlString
        {
            get
            {
                var value = Values.Peek();

                return new ControlString(Index + value.Length, Length - value.Length, new Queue<string>(Values.Skip(1)), Transformers);
            }
        }

        public Queue<string> Values { get;}
        public Queue<string> Transformers { get; }
    }
}