using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RainSeed.Tests.Storage;

[Index(nameof(Token), Name = "token_index", IsUnique = true)]
[Table("tokens")]
public class TokenEntity
{
    [Key] [Column("id")] public long Id { get; set; }

    [Column("token")] public string Token { get; set; }
}
