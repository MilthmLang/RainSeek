using System.Collections.Generic;

namespace RainSeek.Tokenizer
{
    public class BasicTokenizer : ITokenizer
    {
        public IReadOnlyList<string> Delimiters { get; set; } = new[] { " " };

        public bool CaseSensitive { get; set; } = false;

        public IReadOnlyList<Token> Tokenize(string documentId, string input)
        {
            var tokens = new List<Token>();
            int index = 0;

            while (index < input.Length)
            {
                bool isDelimiter = false;
                foreach (var delimiter in Delimiters)
                {
                    if (input.Substring(index).StartsWith(delimiter))
                    {
                        index += delimiter.Length;
                        isDelimiter = true;
                        break;
                    }
                }

                if (isDelimiter) continue;

                int start = index;
                while (index < input.Length)
                {
                    bool atDelimiter = false;
                    foreach (var delimiter in Delimiters)
                    {
                        if (input.Substring(index).StartsWith(delimiter))
                        {
                            atDelimiter = true;
                            break;
                        }
                    }

                    if (atDelimiter) break;
                    index++;
                }

                int end = index - 1;
                var value = input.Substring(start, index - start);

                if (!CaseSensitive)
                {
                    value = value.ToLower();
                }

                tokens.Add(new Token
                {
                    DocumentId = documentId,
                    Value = value,
                    StartPosition = start,
                    EndPosition = end
                });
            }

            return tokens;
        }
    }
}
