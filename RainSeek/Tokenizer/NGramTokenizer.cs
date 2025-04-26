using System.Collections.Generic;
using RainSeek.Indexing;

namespace RainSeek.Tokenizer
{
    public class NGramTokenizer : ITokenizer
    {
        public int N { get; set; } = 3;
        public bool CaseSensitive { get; set; } = false;
        public IReadOnlyList<string> Delimiters { get; set; } = new[] { " " };
        public bool IncludeShortTokens { get; set; } = false;

        public IReadOnlyList<TokenModel> Tokenize(string content)
        {
            var basicTokenizer = new BasicTokenizer()
            {
                CaseSensitive = CaseSensitive,
                Delimiters = Delimiters,
            };

            var basicToken = basicTokenizer.Tokenize(content);

            var nGrams = new List<TokenModel>();

            for (int i = 0; i < basicToken.Count; i++)
            {
                if (basicToken[i].Value.Length < N)
                {
                    if (IncludeShortTokens)
                    {
                        nGrams.Add(new TokenModel
                        {
                            Value = basicToken[i].Value,
                            StartPosition = basicToken[i].StartPosition,
                            EndPosition = basicToken[i].EndPosition
                        });
                    }
                    else
                    {
                        continue;
                    }
                }

                var tokenValue = basicToken[i].Value;
                for (int j = 0; j <= tokenValue.Length - N; j++)
                {
                    var nGramValue = tokenValue.Substring(j, N);
                    nGrams.Add(new TokenModel
                    {
                        Value = nGramValue,
                        StartPosition = basicToken[i].StartPosition + j,
                        EndPosition = basicToken[i].StartPosition + j + N - 1
                    });
                }
            }

            return nGrams;
        }
    }
}
