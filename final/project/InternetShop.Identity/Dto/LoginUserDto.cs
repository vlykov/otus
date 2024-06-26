using System.Text.Json.Serialization;

namespace InternetShop.Identity.Dto;

public class LoginUserDto
{
    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

}
