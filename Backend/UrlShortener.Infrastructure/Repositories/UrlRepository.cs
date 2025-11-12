using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly UrlShortenerDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public UrlRepository(UrlShortenerDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<ShortenedUrl?> GetByCodeAsync(string code)
    {
        // Tạo một key duy nhất cho cache
        string cacheKey = $"shortened_url_{code}";

        // Thử lấy từ cache trước
        if (_memoryCache.TryGetValue(cacheKey, out ShortenedUrl? cachedUrl))
        {
            return cachedUrl;
        }

        // Nếu không có trong cache, truy vấn database
        var shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(s => s.ShortCode == code);

        // Nếu tìm thấy, lưu vào cache
        if (shortenedUrl != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Xóa khỏi cache nếu không được truy cập trong 5 phút
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Xóa khỏi cache sau 1 giờ

            _memoryCache.Set(cacheKey, shortenedUrl, cacheEntryOptions);
        }

        return shortenedUrl;
    }

    public async Task<ShortenedUrl?> GetByLongUrlAsync(string longUrl)
    {
        return await _context.ShortenedUrls.FirstOrDefaultAsync(s => s.OriginalUrl == longUrl);
    }

    public async Task AddAsync(ShortenedUrl shortenedUrl)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrl);

        // Khi một URL mới được thêm, chúng ta có thể xóa cache của nó nếu nó tồn tại
        // để đảm bảo dữ liệu luôn mới (mặc dù trong trường hợp này ít xảy ra)
        string cacheKey = $"shortened_url_{shortenedUrl.ShortCode}";
        _memoryCache.Remove(cacheKey);
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
