using System.Collections.Generic;
using RainSeek.Tokenizer;

namespace RainSeek.Indexing
{
    public class SearchResult
    {
        public string DocumentId { get; set; }
        public List<TokenModel> MatchedTokens { get; set; }
    }
}
