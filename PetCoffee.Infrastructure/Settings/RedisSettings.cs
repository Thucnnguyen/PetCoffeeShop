using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Infrastructure.Settings;

public class RedisSettings
{
    public static readonly string ConfigSection = "Redis";

    [Required]
    public string Host { get; set; } = default!;

    [Required]
    public int Port { get; set; }

    public string? Password { get; set; }
}
