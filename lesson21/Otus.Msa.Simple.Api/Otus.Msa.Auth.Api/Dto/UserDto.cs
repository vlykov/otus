using System.Text.Json.Serialization;

namespace Otus.Msa.Auth.Api.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}

