using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Infrastructure.Settings;

public class JwtSettings
{
    public static readonly string ConfigSection = "Jwt";

    [Required]
    public string Key { get; set; } = default!;

    [Required]
    [Range(1, Int32.MaxValue)]
    public int TokenExpire { get; set; }
}
