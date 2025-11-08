using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlRepository
{
    Task<ShortenedUrl?> GetByCodeAsync(string code);
    Task<ShortenedUrl?> GetByLongUrlAsync(string longUrl);
    Task AddAsync(ShortenedUrl shortenedUrl);
    Task SaveChangesAsync();
    Task<bool> IsCodeUniqueAsync(string code);

    // MỚI: Phương thức lấy URL theo UserId hoặc SessionId
    Task<List<ShortenedUrl>> GetUrlsByUserIdOrSessionIdAsync(string? userId, string? sessionId);
}
