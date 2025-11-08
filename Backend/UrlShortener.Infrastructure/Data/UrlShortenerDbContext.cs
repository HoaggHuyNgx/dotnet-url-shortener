using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Infrastructure.Identity;

namespace UrlShortener.Infrastructure.Data;

public class UrlShortenerDbContext : IdentityDbContext<ApplicationUser>
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortenedUrl>().ToTable("ShortenedUrls");

        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.ShortCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(s => s.ShortCode).IsUnique();
            builder.Property(s => s.OriginalUrl).IsRequired();

            // Index trên SessionId để tăng tốc độ truy vấn
            builder.HasIndex(s => s.SessionId);
        });
    }
}
