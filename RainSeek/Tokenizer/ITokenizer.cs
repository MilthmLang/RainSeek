using System.Collections.Generic;
using System.Linq;
using RainSeek.Indexing;

namespace RainSeek.Tokenizer
{
    public interface ITokenizer
    {
        public IReadOnlyList<TokenModel> Tokenize(string content);
    }
}
