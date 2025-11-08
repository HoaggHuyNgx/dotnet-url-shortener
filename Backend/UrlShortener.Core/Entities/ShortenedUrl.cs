namespace UrlShortener.Core.Entities;

public class ShortenedUrl
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string FullShortUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }

    // Foreign Key cho người dùng đã đăng nhập (chỉ lưu ID)
    public string? UserId { get; set; }

    // ID cho người dùng ẩn danh
    public string? SessionId { get; set; }
}
