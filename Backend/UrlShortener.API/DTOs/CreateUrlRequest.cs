namespace UrlShortener.API.DTOs;

public class CreateUrlRequest
{
    public string OriginalUrl { get; set; } = string.Empty;
    
    public string? CustomCode { get; set; }
}
