using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RainSeek.Indexing;

namespace RainSeek.Tokenizer
{
    public class BasicTokenizer : ITokenizer
    {
        public IReadOnlyList<string> Delimiters { get; set; } = new[]
        {
            "\u0020", "\u00A0",
            "\u1680",
            "\u2000", "\u2001", "\u2002", "\u2003",
            "\u2004", "\u2005", "\u2006", "\u2007",
            "\u2008", "\u2009", "\u200A",
            "\u202F", "\u205F", "\u3000"
        };

        public Func<TokenModel, bool> Predictor { get; set; } = _ => true;

        public bool CaseSensitive { get; set; } = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool MetDelimiter(string content, int index, string delimiter)
        {
            string p = content.Substring(index);

            if (p.Length == 0)
            {
                return false;
            }

            if (p.Length > 0 && delimiter.Length == 1)
            {
                return p[0] == delimiter[0];
            }

            return content.Substring(index).StartsWith(delimiter, StringComparison.InvariantCulture);
        }

        public IReadOnlyList<TokenModel> Tokenize(string content)
        {
            var tokens = new List<TokenModel>();
            int index = 0;

            while (index < content.Length)
            {
                bool isDelimiter = false;
                foreach (var delimiter in Delimiters)
                {
                    if (MetDelimiter(content, index, delimiter))
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
                        if (MetDelimiter(content, index, delimiter))
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
