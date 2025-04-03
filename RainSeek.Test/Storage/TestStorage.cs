using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RainSeek.Storage;
using RainSeek.Tokenizer;

namespace RainSeed.Tests.Storage;

public class TestStorage : IIndexRepository, IDisposable, IAsyncDisposable
{
    private readonly string _name;
    private readonly string _path;
    private readonly TestDBContext _db;

    public TestStorage(string name, string path)
    {
        _name = name;
        _path = path;
        _db = new TestDBContext(_path);
        _db.Database.Migrate();
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }

    public IndexEntry? FindOrNull(string name, string token)
    {
        if (name != _name)
        {
            throw new System.ArgumentException("Invalid name", nameof(name));
        }

        var result = _db.Tokens.FirstOrDefault(s => s.Token == token);
        if (result == null)
        {
            return null;
        }

        var tokenInfo = new List<Token>();
        var tokenDocuments = _db.TokensDocuments.Where(s => s.TokenId == result.Id);
        foreach (var item in tokenDocuments)
        {
            tokenInfo.Add(new Token()
            {
                Id = item.Id,
                Value = token,
                DocumentId = item.DocumentId,
                StartPosition = item.StartPosition,
                EndPosition = item.EndPosition,
            });
        }

        return new IndexEntry()
        {
            Id = result.Id,
            Token = result.Token,
            TokenInfo = tokenInfo
        };
    }

    public void Add(string name, IndexEntry indexEntry)
    {
        if (name != _name)
        {
            throw new System.ArgumentException("Invalid name", nameof(name));
        }

        var token = indexEntry.Token;
        var tokenInfo = _db.Tokens.FirstOrDefault(s => s.Token == token);
        if (tokenInfo == null)
        {
            tokenInfo = new TokenEntity
            {
                Token = token
            };
            _db.Tokens.Add(tokenInfo);
            _db.SaveChanges();
        }

        foreach (var item in indexEntry.TokenInfo)
        {
            if (item.Id > 0)
            {
                // Token already exists, no need to add it again
                continue;
            }
            var tokenDocument = new TokensDocumentsEntity
            {
                DocumentId = item.DocumentId,
                TokenId = tokenInfo.Id,
                StartPosition = item.StartPosition,
                EndPosition = item.EndPosition
            };
            try
            {
                _db.TokensDocuments.Add(tokenDocument);
                _db.SaveChanges();
                item.Id = tokenDocument.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public void Update(string name, IndexEntry existingEntry)
    {
        Add(name, existingEntry);
    }
}
