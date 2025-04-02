using System.Collections.Generic;
using System.Linq;
using RainSeek.Storage;
using RainSeek.Tokenizer;

namespace RainSeek.Indexing
{
    public class IndexService
    {
        private readonly string _name;

        private readonly IReadOnlyList<ITokenizer> _tokenizers;

        private readonly IIndexRepository _indexRepository;

        public IndexService(string name, IReadOnlyList<ITokenizer> tokenizers, IIndexRepository indexRepository)
        {
            _name = name;
            _tokenizers = tokenizers;
            _indexRepository = indexRepository;
        }

        public IndexService(string name, ITokenizer tokenizer, IIndexRepository indexRepository)
        {
            _name = name;
            _tokenizers = new ITokenizer[] { tokenizer };
            _indexRepository = indexRepository;
        }

        public void AddDocument(string documentId, string content)
        {
            var tokens = new List<Token>();

            foreach (var tokenizer in _tokenizers)
            {
                tokens.AddRange(tokenizer.Tokenize(documentId, content));
            }

            var aggregatedTokens = new SortedDictionary<string, IndexEntry>();
            foreach (var token in tokens)
            {
                if (!aggregatedTokens.ContainsKey(token.Value))
                {
                    aggregatedTokens[token.Value] = new IndexEntry
                    {
                        Token = token.Value,
                        TokenInfo = new List<Token> { token }
                    };
                }
                else
                {
                    aggregatedTokens[token.Value].TokenInfo.Add(token);
                }
            }

            foreach (var (token, indexEntry) in aggregatedTokens)
            {
                var existingEntry = _indexRepository.FindOrNull(_name, token);
                if (existingEntry == null)
                {
                    _indexRepository.Add(_name, indexEntry);
                }
                else
                {
                    existingEntry.TokenInfo.AddRange(indexEntry.TokenInfo);
                    _indexRepository.Update(_name, existingEntry);
                }
            }
        }

        public IReadOnlyList<SearchResultEntry> Search(string query)
        {
            var tokens = new List<Token>();

            foreach (var tokenizer in _tokenizers)
            {
                tokens.AddRange(tokenizer.Tokenize("", query));
            }

            var results = new Dictionary<string, SearchResultEntry>();

            foreach (var token in tokens)
            {
                var tokenEntry = _indexRepository.FindOrNull(_name, token.Value);
                if (tokenEntry == null) continue;
                foreach (var item in tokenEntry.TokenInfo)
                {
                    if (!results.ContainsKey(item.DocumentId))
                    {
                        results[item.DocumentId] = new SearchResultEntry
                        {
                            DocumentId = item.DocumentId,
                            MatchedTokens = new List<Token> { item },
                        };
                    }
                    else
                    {
                        results[item.DocumentId].MatchedTokens.Add(item);
                    }
                }
            }

            return results.Values.ToList();
        }
    }
}
