namespace ControlStrings
{
    using System;

    public class ValueControlStringMatcher : ContextControlStringMatcher
    {
        public ValueControlStringMatcher(string context, Func<string> valueGetter) : base(context, new FuncControlStringMatcher(x => valueGetter(), x => true))
        {
        }
    }
}