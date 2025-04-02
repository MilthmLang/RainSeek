using System.Collections.Generic;
using RainSeek.Tokenizer;

namespace RainSeek.Storage
{
    public class IndexEntry
    {
        public long Id = -1;
        public string Token;
        public List<Token> TokenInfo;
    }
}
