using System.Text.Json.Serialization;

namespace Otus.Msa.Auth.Api.Dto;

public class LoginUserDto
{
    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

}
