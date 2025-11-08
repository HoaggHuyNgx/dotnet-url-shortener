using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlService
{
    Task<ShortenedUrl> ShortenUrlAsync(string originalUrl, string requestScheme, string requestHost, string? customCode = null, string? userId = null, string? sessionId = null);
    Task<List<ShortenedUrl>> GetUrlsForUserAsync(string? userId, string? sessionId);
}
