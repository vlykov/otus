using System.Text.Json.Serialization;

namespace Otus.Msa.Profile.Api.Dto;

public class UpdateUserProfileDto
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }
}

