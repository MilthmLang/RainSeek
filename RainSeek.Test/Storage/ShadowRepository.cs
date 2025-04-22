using System.Collections.Generic;
using RainSeek.Storage;

namespace RainSeed.Tests.Storage;

public class ShadowRepository : IIndexRepository
{
    private readonly RepositoryFactory _repositoryFactory;

    public ShadowRepository(RepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    private IIndexRepository GetRepository(string indexName)
    {
        return _repositoryFactory.Get(indexName);
    }

    public RainSeek.Storage.TokenEntity? FindTokenByContent(string indexName, string tokenValue)
    {
        return GetRepository(indexName).FindTokenByContent(indexName, tokenValue);
    }

    public RainSeek.Storage.TokenEntity AddToken(string indexName, string tokenValue)
    {
        return GetRepository(indexName).AddToken(indexName, tokenValue);
    }

    public IReadOnlyList<RainSeek.Storage.DocumentTokenEntity> FindDocumentTokenByTokenId(string indexName,
        long tokenId)
    {
        return GetRepository(indexName).FindDocumentTokenByTokenId(indexName, tokenId);
    }

    public RainSeek.Storage.DocumentTokenEntity AddDocumentToken(string indexName, long tokenId, string documentId,
        int startPosition, int endPosition)
    {
        return GetRepository(indexName).AddDocumentToken(indexName, tokenId, documentId, startPosition, endPosition);
    }
}
