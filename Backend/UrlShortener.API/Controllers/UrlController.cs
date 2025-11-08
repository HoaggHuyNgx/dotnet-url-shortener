using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.API.DTOs;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/urls")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost]
    public async Task<IActionResult> ShortenUrl([FromBody] CreateUrlRequest request)
    {
        string? userId = null;
        string? sessionId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            // SỬA LỖI: Sử dụng ClaimTypes.NameIdentifier để lấy ID duy nhất của user
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        else
        {
            sessionId = HttpContext.Request.Headers["X-Session-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(sessionId))
            {
                // Không trả về lỗi, vì có thể là request cũ không có session
                // Logic service sẽ xử lý việc không có ID
            }
        }

        if (!string.IsNullOrWhiteSpace(request.CustomCode) && userId == null)
        {
            return Unauthorized("You must be logged in to use a custom code.");
        }

        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
        {
            return BadRequest("URL cannot be empty.");
        }

        try
        {
            var shortenedUrl = await _urlService.ShortenUrlAsync(request.OriginalUrl, Request.Scheme, Request.Host.ToString(), request.CustomCode, userId, sessionId);

            var response = new UrlResponse
            {
                Id = shortenedUrl.Id,
                LongUrl = shortenedUrl.OriginalUrl,
                ShortUrl = shortenedUrl.FullShortUrl,
                Code = shortenedUrl.ShortCode,
                CreatedOnUtc = shortenedUrl.CreatedOnUtc
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("my-urls")]
    public async Task<IActionResult> GetMyUrls()
    {
        string? userId = null;
        string? sessionId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            // SỬA LỖI: Sử dụng ClaimTypes.NameIdentifier để lấy ID duy nhất của user
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        else
        {
            sessionId = HttpContext.Request.Headers["X-Session-Id"].FirstOrDefault();
        }

        var urls = await _urlService.GetUrlsForUserAsync(userId, sessionId);

        var response = urls.Select(url => new UrlResponse
        {
            Id = url.Id,
            LongUrl = url.OriginalUrl,
            ShortUrl = url.FullShortUrl,
            Code = url.ShortCode,
            CreatedOnUtc = url.CreatedOnUtc
        }).ToList();

        return Ok(response);
    }
}
