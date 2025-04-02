using System.Collections.Generic;

namespace RainSeek.Tokenizer
{
    public class NGramTokenizer : ITokenizer
    {
        public int N { get; set; } = 3;
        public bool CaseSensitive { get; set; } = false;
        public IReadOnlyList<string> Delimiters { get; set; } = new[] { " " };

        public IReadOnlyList<Token> Tokenize(string documentId, string input)
        {
            var basicTokenizer = new BasicTokenizer()
            {
                CaseSensitive = CaseSensitive,
                Delimiters = Delimiters,
            };

            var basicToken = basicTokenizer.Tokenize(documentId, input);
            
            var nGrams = new List<Token>();

            for (int i = 0; i < basicToken.Count; i++)
            {
                var tokenValue = basicToken[i].Value;
                for (int j = 0; j <= tokenValue.Length - N; j++)
                {
                    var nGramValue = tokenValue.Substring(j, N);
                    nGrams.Add(new Token
                    {
                        DocumentId = documentId,
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
