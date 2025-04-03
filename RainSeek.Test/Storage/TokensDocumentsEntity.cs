using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RainSeed.Tests.Storage;

[Index(nameof(TokenId), nameof(DocumentId), Name = "token_id_document_id")]
[Index(nameof(DocumentId), nameof(TokenId), Name = "document_id_token_id")]
[Index(nameof(TokenId), nameof(DocumentId), nameof(StartPosition), nameof(EndPosition)
    , Name = "unique")]
[Table("tokens_documents")]
public class TokensDocumentsEntity
{
    [Key] [Column("id")] public long Id { get; set; }

    [Column("token_id")] public long TokenId { get; set; }

    [Column("document_id")] public string DocumentId { get; set; }

    [Column("start_position")] public int StartPosition { get; set; }

    [Column("end_position")] public int EndPosition { get; set; }
}
