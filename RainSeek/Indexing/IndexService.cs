using System.Collections.Generic;
using System.Linq;
using System.Text;
using RainSeek.Storage;
using RainSeek.Tokenizer;

namespace RainSeek.Indexing
{
    public class IndexService
    {
        private readonly string _indexName;

        private readonly IReadOnlyList<ITokenizer> _tokenizers;

        private readonly IIndexRepository _indexRepository;

        public IndexService(string indexName, IReadOnlyList<ITokenizer> tokenizers, IIndexRepository indexRepository)
        {
            _indexName = indexName;
            _tokenizers = tokenizers;
            _indexRepository = indexRepository;
        }

        public IndexService(string indexName, ITokenizer tokenizer, IIndexRepository indexRepository)
        {
            _indexName = indexName;
            _tokenizers = new ITokenizer[] { tokenizer };
            _indexRepository = indexRepository;
        }

        private List<TokenModel> Tokenize(string content)
        {
            var tokens = new List<TokenModel>();
            foreach (var tokenizer in _tokenizers)
            {
                tokens.AddRange(tokenizer.Tokenize(content));
            }

            return tokens;
        }

        public void AddDocument(string documentId, List<string> content)
        {
            var sb = new StringBuilder(64);
            foreach (var item in content)
            {
                sb.Append(item).Append("\n");
            }

            AddDocument(documentId, sb.ToString());
        }

        public void AddDocument(string documentId, string content)
        {
            var tokens = Tokenize(content);
            LinkDocumentToToken(documentId, tokens);
        }

        private void LinkDocumentToToken(string documentId, List<TokenModel> tokens)
        {
            foreach (var token in tokens)
            {
                var tokenEntity = _indexRepository.FindTokenByContent(_indexName, token.Value)
                                  ?? _indexRepository.AddToken(_indexName, token.Value);

                _indexRepository.AddDocumentToken(
                    _indexName, tokenEntity.ID, documentId, token.StartPosition, token.EndPosition
                );
            }
        }

        public IReadOnlyList<SearchResult> Search(string query)
        {
            var tokens = Tokenize(query);
            return Search(tokens);
        }

        public IReadOnlyList<SearchResult> Search(IReadOnlyCollection<TokenModel> tokens)
        {
            var strs = tokens.Select(t => t.Value).ToList();
            return Search(strs);
        }

        public IReadOnlyList<SearchResult> Search(IReadOnlyCollection<string> tokens)
        {
            var results = new Dictionary<string, SearchResult>();

            foreach (var token in tokens)
            {
                var tokenEntity = _indexRepository.FindTokenByContent(_indexName, token);
                if (tokenEntity == null)
                {
                    continue;
                }

                var documentsToken = _indexRepository.FindDocumentTokenByTokenId(_indexName, tokenEntity.ID);

                foreach (var item in documentsToken)
                {
                    var tokenModel = new TokenModel()
                    {
                        Value = tokenEntity.Content,
                        StartPosition = item.StartPosition,
                        EndPosition = item.EndPosition
                    };

                    if (!results.ContainsKey(item.DocumentID))
                    {
                        results[item.DocumentID] = new SearchResult
                        {
                            DocumentId = item.DocumentID,
                            MatchedTokens = new List<TokenModel> { tokenModel },
                        };
                    }
                    else
                    {
                        results[item.DocumentID].MatchedTokens.Add(tokenModel);
                    }
                }
            }

            return results.Values.ToList();
        }
    }
}
