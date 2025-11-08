using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UrlShortener.Core.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;
    private const int CodeLength = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private readonly Random _random = new();

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    public async Task<ShortenedUrl> ShortenUrlAsync(string originalUrl, string requestScheme, string requestHost, string? customCode = null, string? userId = null, string? sessionId = null)
    {
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("The provided URL is not valid.");
        }

        string finalCode;

        if (!string.IsNullOrWhiteSpace(customCode))
        {
            if (!IsCustomCodeValid(customCode))
            {
                throw new ArgumentException("Custom short code contains invalid characters. Use only letters, numbers, and hyphens.");
            }

            if (!await _urlRepository.IsCodeUniqueAsync(customCode))
            {
                throw new InvalidOperationException("This custom short code is already in use.");
            }
            finalCode = customCode;
        }
        else
        {
            do
            {
                finalCode = GenerateUniqueShortCode();
            }
            while (!await _urlRepository.IsCodeUniqueAsync(finalCode));
        }

        var shortenedUrl = new ShortenedUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = finalCode,
            FullShortUrl = $"{requestScheme}://{requestHost}/{finalCode}",
            CreatedOnUtc = DateTime.UtcNow,
            UserId = userId,
            SessionId = sessionId
        };

        await _urlRepository.AddAsync(shortenedUrl);
        await _urlRepository.SaveChangesAsync();

        return shortenedUrl;
    }

    public async Task<List<ShortenedUrl>> GetUrlsForUserAsync(string? userId, string? sessionId)
    {
        if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(sessionId))
        {
            return new List<ShortenedUrl>(); // Không có ID nào để tìm kiếm
        }
        return await _urlRepository.GetUrlsByUserIdOrSessionIdAsync(userId, sessionId);
    }

    private string GenerateUniqueShortCode()
    {
        var codeChars = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
        {
            var randomIndex = _random.Next(Alphabet.Length);
            codeChars[i] = Alphabet[randomIndex];
        }
        return new string(codeChars);
    }

    private bool IsCustomCodeValid(string code)
    {
        return Regex.IsMatch(code, @"^[a-zA-Z0-9-]+$");
    }
}
