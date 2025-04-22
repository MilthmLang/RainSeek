using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeek.Indexing;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Tokenizer;

[TestClass]
public class BasicTokenizerTest
{
    [TestMethod]
    public void SimpleTest()
    {
        var tokenizer = new BasicTokenizer
        {
            Delimiters = [" "],
            CaseSensitive = true,
        };
        var tokens = tokenizer.Tokenize("Hello World");
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(new TokenModel()
        {
            Value = "Hello",
            StartPosition = 0,
            EndPosition = 4,
        }, tokens[0]);
        Assert.AreEqual(new TokenModel()
        {
            Value = "World",
            StartPosition = 6,
            EndPosition = 10,
        }, tokens[1]);
    }

    [TestMethod]
    public void MultiDelimiters()
    {
        var tokenizer = new BasicTokenizer
        {
            Delimiters = [" ", ",", "!"],
            CaseSensitive = true,
        };
        var tokens = tokenizer.Tokenize("Hello, World!");
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(new TokenModel()
        {
            Value = "Hello",
            StartPosition = 0,
            EndPosition = 4,
        }, tokens[0]);
        Assert.AreEqual(new TokenModel()
        {
            Value = "World",
            StartPosition = 7,
            EndPosition = 11,
        }, tokens[1]);
    }
}
