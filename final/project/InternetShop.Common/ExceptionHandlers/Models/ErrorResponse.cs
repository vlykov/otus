using System.Text.Json.Serialization;

namespace InternetShop.Common.ExceptionHandlers.Models;

/// <summary>
///		Описывает DTO для API ответов с ошибкой.
/// </summary>
public sealed record ErrorResponse
{
    /// <summary>
    ///		Текст ошибки.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; }

    /// <summary>
    ///		Детализация ошибки.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; }

    public ErrorResponse(string message, string? details = null)
    {
        Message = message;
        Details = details;
    }
}
