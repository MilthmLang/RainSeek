using System.Collections.Generic;
using RainSeek.Indexing;

namespace RainSeek.Storage
{
    public interface IIndexRepository
    {
        TokenEntity? FindTokenByContent(string indexName, string tokenValue);

        TokenEntity AddToken(string indexName, string tokenValue);

        IReadOnlyList<DocumentTokenEntity> FindDocumentTokenByTokenId(string indexName, long tokenId);

        DocumentTokenEntity AddDocumentToken(string indexName, long tokenId, string documentId, int startPosition,
            int endPosition);
    }
}
