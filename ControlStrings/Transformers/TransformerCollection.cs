using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ControlStrings
{
    public class TransformerCollection : ITransformer, IEnumerable<ITransformer>
    {
        private readonly IEnumerable<ITransformer> transformers;

        public TransformerCollection(IEnumerable<ITransformer> transformers)
        {
            this.transformers = transformers ?? throw new System.ArgumentNullException(nameof(transformers));
        }

        public IEnumerator<ITransformer> GetEnumerator() => transformers.GetEnumerator();

        public bool Matches(string transformCode)
        {
            if (transformCode is null)
            {
                throw new System.ArgumentNullException(nameof(transformCode));
            }

            return transformers.Any(x => x.Matches(transformCode));
        }

        public string Transform(string transformCode, string input)
        {
            if (transformCode is null)
            {
                throw new System.ArgumentNullException(nameof(transformCode));
            }

            if (input is null)
            {
                throw new System.ArgumentNullException(nameof(input));
            }

            var matchingTransformers = transformers.Where(x => x.Matches(transformCode));

            if(!matchingTransformers.Any())
            {
                throw new MissingTransformerException($"Could not find a transformer that matched the string {transformCode}");
            }

            return matchingTransformers.First().Transform(transformCode, input);
        }

        IEnumerator IEnumerable.GetEnumerator() => transformers.GetEnumerator();
    }
}