using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RainSeek.Storage;

namespace RainSeed.Tests.Storage;

public class EntityFrameworkRepository : IIndexRepository
{
    private readonly string _indexName;
    private readonly TestDBContext _db;

    private readonly string tokenEntityTableName;

    private readonly string documentsTokensTableName;

    private readonly string createTokenEntityTable;

    private readonly string createDocumentsTokensEntityTable;

    public EntityFrameworkRepository(string indexName, TestDBContext db)
    {
        _indexName = indexName;
        _db = db;

        tokenEntityTableName = $"{indexName}_tokens";
        documentsTokensTableName = $"{indexName}_documents_tokens";
        createTokenEntityTable = $"""
                                      CREATE TABLE IF NOT EXISTS {tokenEntityTableName} (
                                          id INTEGER PRIMARY KEY,
                                          content TEXT NOT NULL,
                                          UNIQUE(content)
                                      );
                                  """;
        createDocumentsTokensEntityTable = $"""
                                                CREATE TABLE IF NOT EXISTS {documentsTokensTableName} (
                                                    id INTEGER PRIMARY KEY,
                                                    token_id INTEGER NOT NULL,
                                                    document_id TEXT NOT NULL,
                                                    start_position INTEGER NOT NULL,
                                                    end_position INTEGER NOT NULL,
                                                    UNIQUE(token_id, document_id, start_position, end_position)
                                                );
                                            """;

        var t =_db.Database.ExecuteSqlRaw(createTokenEntityTable);
        _db.Database.ExecuteSqlRaw(createDocumentsTokensEntityTable);
    }

    private long GetLastInsertRowId()
    {
        using var command = _db.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT last_insert_rowid();";

        if (command.Connection.State != System.Data.ConnectionState.Open)
        {
            command.Connection.Open();
        }

        var result = command.ExecuteScalar();
        return Convert.ToInt64(result);
    }

    public RainSeek.Indexing.TokenEntity? FindTokenByContent(string indexName, string tokenValue)
    {
        if (_indexName != indexName)
        {
            throw new Exception($"Index name mismatch: {_indexName} != {indexName}");
        }

        var sql = $"SELECT * FROM {tokenEntityTableName} WHERE content = @p0 LIMIT 1";
        var entity = _db.Set<TokenEntity>().FromSqlRaw(sql, tokenValue).FirstOrDefault();
        if (entity == null)
        {
            return null;
        }

        return new RainSeek.Indexing.TokenEntity()
        {
            Id = entity.Id,
            Content = entity.Content,
        };
    }

    public RainSeek.Indexing.TokenEntity AddToken(string indexName, string tokenValue)
    {
        if (_indexName != indexName)
        {
            throw new Exception($"Index name mismatch: {_indexName} != {indexName}");
        }

        var sql = $"INSERT INTO {tokenEntityTableName} (content) VALUES (@p0);";
        _db.Database.ExecuteSqlRaw(sql, tokenValue);

        var id = GetLastInsertRowId();
        sql = $"SELECT * FROM {tokenEntityTableName} WHERE id = @p0";
        var entity = _db.Set<TokenEntity>().FromSqlRaw(sql, id).FirstOrDefault();

        if (entity == null)
        {
            throw new Exception($"entity not found after insert: {id}");
        }

        return new RainSeek.Indexing.TokenEntity()
        {
            Id = entity.Id,
            Content = entity.Content,
        };
    }

    public IReadOnlyList<RainSeek.Indexing.DocumentTokenEntity> FindDocumentTokenByTokenId(string indexName,
        long tokenId)
    {
        if (_indexName != indexName)
        {
            throw new Exception($"Index name mismatch: {_indexName} != {indexName}");
        }

        var sql = $"SELECT * FROM {documentsTokensTableName} WHERE token_id = @p0";
        var entities = _db.Set<DocumentTokenEntity>().FromSqlRaw(sql, tokenId).ToList();

        return entities.Select(s =>
            new RainSeek.Indexing.DocumentTokenEntity()
            {
                Id = s.Id,
                TokenId = s.TokenId,
                DocumentId = s.DocumentId,
                StartPosition = s.StartPosition,
                EndPosition = s.EndPosition,
            }
        ).ToList();
    }

    public RainSeek.Indexing.DocumentTokenEntity AddDocumentToken(string indexName,
        long tokenId,
        string documentId,
        int startPosition,
        int endPosition)
    {
        if (_indexName != indexName)
        {
            throw new Exception($"Index name mismatch: {_indexName} != {indexName}");
        }

        var sql =
            $"INSERT INTO {documentsTokensTableName} (token_id, document_id, start_position, end_position) VALUES (@p0, @p1, @p2, @p3)";
        _db.Database.ExecuteSqlRaw(sql, tokenId, documentId, startPosition, endPosition);

        var id = GetLastInsertRowId();
        sql = $"SELECT * FROM {documentsTokensTableName} WHERE id = @p0";
        var entity = _db.Set<DocumentTokenEntity>().FromSqlRaw(sql, id).FirstOrDefault();

        if (entity == null)
        {
            throw new Exception($"entity not found after insert: {id}");
        }

        return new RainSeek.Indexing.DocumentTokenEntity()
        {
            Id = entity.Id,
            TokenId = entity.TokenId,
            DocumentId = entity.DocumentId,
            StartPosition = entity.StartPosition,
            EndPosition = entity.EndPosition,
        };
    }
}
