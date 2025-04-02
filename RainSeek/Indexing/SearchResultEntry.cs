using System.Collections.Generic;
using RainSeek.Tokenizer;

public class SearchResultEntry
{
    public string DocumentId { get; set; }
    public List<Token> MatchedTokens { get; set; }
}
