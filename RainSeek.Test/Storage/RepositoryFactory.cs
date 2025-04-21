using System;
using System.Collections.Concurrent;
using RainSeek.Storage;

namespace RainSeed.Tests.Storage;

public class RepositoryFactory
{
    private readonly Func<string, IIndexRepository> _factory;
    private readonly ConcurrentDictionary<string, IIndexRepository> _repositories = new();

    public RepositoryFactory(Func<string, IIndexRepository> factory)
    {
        _factory = factory;
    }

    public IIndexRepository Get(string indexName)
    {
        return _repositories.GetOrAdd(indexName, _factory);
    }
}
