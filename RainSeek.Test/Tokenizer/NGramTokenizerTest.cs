using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Tokenizer;

[TestClass]
public class NGramTokenizerTest
{
    [TestMethod]
    public void SimpleTest()
    {
        var tokenizer = new NGramTokenizer()
        {
            N = 3,
            Delimiters = [" "],
            CaseSensitive = true,
        };
        var tokens = tokenizer.Tokenize("", "Hello World");
        Assert.AreEqual(6, tokens.Count);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "Hel",
            StartPosition = 0,
            EndPosition = 2,
        }, tokens[0]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "ell",
            StartPosition = 1,
            EndPosition = 3,
        }, tokens[1]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "llo",
            StartPosition = 2,
            EndPosition = 4,
        }, tokens[2]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "Wor",
            StartPosition = 6,
            EndPosition = 8,
        }, tokens[3]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "orl",
            StartPosition = 7,
            EndPosition = 9,
        }, tokens[4]);
        Assert.AreEqual(new Token()
        {
            DocumentId = "",
            Value = "rld",
            StartPosition = 8,
            EndPosition = 10,
        }, tokens[5]);
    }
}
