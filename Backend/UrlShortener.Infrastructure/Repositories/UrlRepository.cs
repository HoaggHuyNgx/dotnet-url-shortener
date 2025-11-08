using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly UrlShortenerDbContext _context;

    public UrlRepository(UrlShortenerDbContext context)
    {
        _context = context;
    }

    public async Task<ShortenedUrl?> GetByCodeAsync(string code)
    {
        return await _context.ShortenedUrls.FirstOrDefaultAsync(s => s.ShortCode == code);
    }

    public async Task<ShortenedUrl?> GetByLongUrlAsync(string longUrl)
    {
        return await _context.ShortenedUrls.FirstOrDefaultAsync(s => s.OriginalUrl == longUrl);
    }

    public async Task AddAsync(ShortenedUrl shortenedUrl)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrl);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsCodeUniqueAsync(string code)
    {
        return !await _context.ShortenedUrls.AnyAsync(u => u.ShortCode == code);
    }

    public async Task<List<ShortenedUrl>> GetUrlsByUserIdOrSessionIdAsync(string? userId, string? sessionId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            return await _context.ShortenedUrls
                                 .Where(u => u.UserId == userId)
                                 .OrderByDescending(u => u.CreatedOnUtc)
                                 .ToListAsync();
        }
        else if (!string.IsNullOrEmpty(sessionId))
        {
            return await _context.ShortenedUrls
                                 .Where(u => u.SessionId == sessionId)
                                 .OrderByDescending(u => u.CreatedOnUtc)
                                 .ToListAsync();
        }
        return new List<ShortenedUrl>();
    }
}
