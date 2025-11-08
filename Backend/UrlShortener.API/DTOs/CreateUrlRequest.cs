using System.ComponentModel.DataAnnotations;

namespace UrlShortener.API.DTOs;

public class CreateUrlRequest
{
    [Required]
    public string OriginalUrl { get; set; } = string.Empty;
    
    // MỚI: Tùy chọn cho người dùng
    public string? CustomCode { get; set; }
}
