using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeed.Tests.Storage;
using RainSeek.Indexing;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Indexing;

[TestClass]
public class NGramTokenizerIndexingTest
{
    private static TestDBContext _db;
    private static IndexService _indexService = null!;
    private static List<TestDocument> _documents = null!;
    private static EntityFrameworkRepository _storage;

    [ClassInitialize]
    public static void Init(TestContext ctx)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "ngram_tokenizer_indexing_test.db");
        File.Delete(path);
        var db = new TestDBContext(path);
        var storage = new EntityFrameworkRepository("ngram_tokenizer_indexing_test", db);
        var delimiters = new[]
        {
            " ", ",", ".", "!", "?", "、", "。", "！", "？"
        };
        var tokenizers = new[]
        {
            new NGramTokenizer()
            {
                N = 3,
                Delimiters = delimiters,
                CaseSensitive = false
            },
            new NGramTokenizer()
            {
                N = 2,
                Delimiters = delimiters,
                CaseSensitive = false
            },
            new NGramTokenizer()
            {
                N = 1,
                Delimiters = delimiters,
                CaseSensitive = false
            },
        };
        var indexService = new IndexService("ngram_tokenizer_indexing_test", tokenizers, storage);

        _db = db;
        _storage = storage;
        _indexService = indexService;
        _documents =
        [
            new TestDocument() { Id = "1", Content = "The quick brown fox jumps over the lazy dog" },
            new TestDocument() { Id = "2", Content = "A journey of a thousand miles begins with a single step" },
            new TestDocument() { Id = "3", Content = "To be or not to be, that is the question" },
            new TestDocument() { Id = "4", Content = "All that glitters is not gold" },
            new TestDocument() { Id = "5", Content = "The only thing we have to fear is fear itself" },
            new TestDocument() { Id = "6", Content = "人類社会のすべての構成員の固有の尊厳と平等で譲ることのできない権利とを承認することは" },
            new TestDocument() { Id = "7", Content = "모든 인류 구성원의 천부의 존엄성과 동등하고 양도할 수 없는 권리를 인정하는" },
            new TestDocument() { Id = "8", Content = "鉴于对人类家庭所有成员的固有尊严及其平等的和不移的权利的承认，乃是世界自由、正义与和平的基础" },
            new TestDocument() { Id = "9", Content = "鑑於對人類家庭所有成員的固有尊嚴及其平等的和不移的權利的承認，乃是世界自由、正義與和平的基礎" },
        ];

        foreach (var document in _documents)
        {
            _indexService.AddDocument(document.Id, document.Content);
        }
    }

    [ClassCleanup]
    public static void Destroy()
    {
        _db.Dispose();
    }

    [TestMethod]
    public void Search1()
    {
        var result = _indexService.Search("The quick brown fox jumps over the lazy dog");
        result = result.OrderByDescending(s => s.MatchedTokens.Count).ToList();
        Assert.IsTrue(result.Count >= 1);
        Assert.AreEqual("1", result[0].DocumentId);
    }

    [TestMethod]
    public void Search2()
    {
        var result = _indexService.Search("人类");
        result = result.OrderByDescending(s => s.MatchedTokens.Count).ToList();
        Assert.IsTrue(result.Count >= 1);
        Assert.AreEqual("8", result[0].DocumentId);
    }
}
