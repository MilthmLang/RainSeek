using System.Collections.Generic;
using System.Linq;

namespace RainSeek.Tokenizer
{
    public interface ITokenizer
    {
        public IReadOnlyList<Token> Tokenize(string documentId, string input);
    }
}
