using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Tokenizer;

[TestClass]
public class BasicTokenizerTest
{
    [TestMethod]
    public void SimpleTest()
    {
        var tokenizer = new BasicTokenizer();
        tokenizer.Delimiters = new[] { " " };
        var tokens = tokenizer.Tokenize("", "Hello World");
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "Hello",
            StartPosition = 0,
            EndPosition = 4
        }, tokens[0]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "World",
            StartPosition = 6,
            EndPosition = 10
        }, tokens[1]);
    }

    [TestMethod]
    public void MultiDelimiters()
    {
        var tokenizer = new BasicTokenizer();
        tokenizer.Delimiters = new[] { " ", ",", "!" };
        var tokens = tokenizer.Tokenize("", "Hello, World!");
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "Hello",
            StartPosition = 0,
            EndPosition = 4
        }, tokens[0]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "World",
            StartPosition = 7,
            EndPosition = 11
        }, tokens[1]);
    }
}
