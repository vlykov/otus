using System.Text.Json.Serialization;

namespace Otus.Msa.Auth.Api.Dto;

public class RegisterUserDto
{
    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
