using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeek.Indexing;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Application;

[TestClass]
public class ApplicationTest
{
    private static IndexService _indexService = null!;
    private static List<TestDocument> _documents = null!;
    private static TestStorage _storage;

    [ClassInitialize]
    public static void Init(TestContext ctx)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "test_index.db");
        File.Delete(path);
        var storage = new TestStorage("test_index", path);
        var tokenizers = new[]
        {
            new BasicTokenizer()
            {
                Delimiters = [" ", ",", ".", "!", "?"],
                CaseSensitive = false
            }
        };
        var indexService = new IndexService("test_index", tokenizers, storage);

        _storage = storage;
        _indexService = indexService;
        _documents =
        [
            new TestDocument() { Id = "1", Content = "The quick brown fox jumps over the lazy dog" },
            new TestDocument() { Id = "2", Content = "A journey of a thousand miles begins with a single step" },
            new TestDocument() { Id = "3", Content = "To be or not to be, that is the question" },
            new TestDocument() { Id = "4", Content = "All that glitters is not gold" },
            new TestDocument() { Id = "5", Content = "The only thing we have to fear is fear itself" }
        ];

        foreach (var document in _documents)
        {
            _indexService.AddDocument(document.Id, document.Content);
        }
    }

    [ClassCleanup]
    public static void Destroy()
    {
        _storage.Dispose();
    }

    [TestMethod]
    public void ExtactSearch()
    {
        var result = _indexService.Search("The quick brown fox jumps over the lazy dog");
        result = result.OrderByDescending(s => s.MatchedTokens.Count).ToList();
        Assert.IsTrue(result.Count >= 1);
        Assert.AreEqual("1", result[0].DocumentId);
    }
}
