using System.Text.Json.Serialization;

namespace Otus.Msa.Profile.Api.Dto;

public class UserExtendedProfileDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }
}

