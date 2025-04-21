using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RainSeed.Tests.Storage;

public class TokenEntity
{
    [Key] [Column("id")] public long Id { get; set; }

    [Column("content")] public string Content { get; set; }
}
