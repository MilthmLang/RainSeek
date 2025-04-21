using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RainSeed.Tests.Storage;

public class DocumentTokenEntity
{
    [Key] [Column("id")] public long Id { get; set; }

    [Column("token_id")] public long TokenId { get; set; }

    [Column("document_id")] public string DocumentId { get; set; }

    [Column("start_position")] public int StartPosition { get; set; }

    [Column("end_position")] public int EndPosition { get; set; }
}
