using System;
using System.Collections.Generic;

namespace RainSeek.Tokenizer
{
    public class LineTokenizer : ITokenizer
    {
        private readonly IReadOnlyList<string> _delimiters;
        private readonly Func<TokenModel, bool> _predictor;
        private readonly bool _caseSensitive;

        private BasicTokenizer _tokenizer;

        public LineTokenizer(IReadOnlyList<string> delimiters,
            Func<TokenModel, bool> predictor,
            bool caseSensitive
        )
        {
            _delimiters = delimiters;
            _predictor = predictor;
            _caseSensitive = caseSensitive;
            _tokenizer = new BasicTokenizer()
            {
                Delimiters = new[] { "\r", "\n", "\r\n" },
                CaseSensitive = caseSensitive,
                Predictor = predictor
            };
        }

        public IReadOnlyList<TokenModel> Tokenize(string content)
        {
            var tokens = _tokenizer.Tokenize(content);

            var result = new List<TokenModel>();
            foreach (var token in tokens)
            {
                bool containsDelimiter = false;
                foreach (var delimiter in _delimiters)
                {
                    if (token.Value.Contains(delimiter))
                    {
                        containsDelimiter = true;
                        break;
                    }
                }

                if (!containsDelimiter)
                {
                    result.Add(token);
                }
            }

            return result;
        }
    }
}
