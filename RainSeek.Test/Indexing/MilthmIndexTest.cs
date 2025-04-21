using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeed.Tests.Storage;
using RainSeek.Indexing;
using RainSeek.Storage;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Indexing;

public class MilthmIndexTest
{
    private static TestDBContext _db = null!;

    private static IIndexRepository _storage = null!;

    private static IndexService _titleDelimiterIndexing = null!;

    [ClassInitialize]
    public static void Init(TestContext ctx)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "document-index.db");
        _db = new TestDBContext(path);

        var factory = new RepositoryFactory(s => new EntityFrameworkRepository(s, _db));
        _storage = new ShadowRepository(factory);

        var delimitersList = new[]
        {
            " ", "#", "~", "-", "(", ")", "?", ".", "\"", "!", ",", "\r", "\n", "+", ".", "_", "†", "・",
        };

        var basicDelimitersTokenizer = new BasicTokenizer()
        {
            Delimiters = delimitersList,
            Predictor = s => s.Value.Length >= 3,
        };
        var nameDelimitersTokenizer = new BasicTokenizer()
        {
            Delimiters = delimitersList,
        };

        var NGram3Tokenizer = new NGramTokenizer()
        {
            Delimiters = delimitersList,
            N = 2,
        };

        _titleDelimiterIndexing = new IndexService("title_delimiter", nameDelimitersTokenizer, _storage);
    }

    [ClassCleanup]
    public static void Destroy()
    {
        _db.Dispose();
    }

    [TestMethod]
    public void Search1()
    {
        var result = _titleDelimiterIndexing.Search("樱落繁花");
        result = result.OrderByDescending(s => s.MatchedTokens.Count).ToList();
        Assert.IsTrue(result.Count >= 3);
    }

    [TestMethod]
    public void Search2()
    {
        var result = _titleDelimiterIndexing.Search("命日 时落之雨");
        result = result.OrderByDescending(s => s.MatchedTokens.Count).ToList();
        Assert.IsTrue(result.Count >= 6);
    }
}
