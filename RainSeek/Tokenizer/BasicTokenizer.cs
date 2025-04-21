using System;
using System.Collections.Generic;
using RainSeek.Indexing;

namespace RainSeek.Tokenizer
{
    public class BasicTokenizer : ITokenizer
    {
        public IReadOnlyList<string> Delimiters { get; set; } = new[] { " " };
        public Func<TokenModel, bool> Predictor { get; set; } = _ => true;

        public bool CaseSensitive { get; set; } = false;

        public IReadOnlyList<TokenModel> Tokenize(string content)
        {
            var tokens = new List<TokenModel>();
            int index = 0;

            while (index < content.Length)
            {
                bool isDelimiter = false;
                foreach (var delimiter in Delimiters)
                {
                    if (content.Substring(index).StartsWith(delimiter))
                    {
                        index += delimiter.Length;
                        isDelimiter = true;
                        break;
                    }
                }

                if (isDelimiter) continue;

                int start = index;
                while (index < content.Length)
                {
                    bool atDelimiter = false;
                    foreach (var delimiter in Delimiters)
                    {
                        if (content.Substring(index).StartsWith(delimiter))
                        {
                            atDelimiter = true;
                            break;
                        }
                    }

                    if (atDelimiter) break;
                    index++;
                }

                int end = index - 1;
                var value = content.Substring(start, index - start);

                if (!CaseSensitive)
                {
                    value = value.ToLower();
                }

                var newToken = new TokenModel
                {
                    Value = value,
                    StartPosition = start,
                    EndPosition = end
                };
                if (Predictor(newToken))
                {
                    tokens.Add(newToken);
                }
            }

            return tokens;
        }
    }
}
