using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.Data;
using UrlShortener.Infrastructure.Identity; // Thêm using mới

var builder = WebApplication.CreateBuilder(args);

// --- Cấu hình CORS ---
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// --- Cấu hình Database và Infrastructure ---
builder.Services.AddInfrastructure(builder.Configuration);

// --- Cấu hình ASP.NET Core Identity ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<UrlShortenerDbContext>()
    .AddDefaultTokenProviders();

// --- Cấu hình JWT Authentication ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// --- Đăng ký các dịch vụ khác ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUrlService, UrlService>();

var app = builder.Build();

// --- Cấu hình HTTP Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

// Kích hoạt Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/{code}", async (string code, IUrlRepository urlRepository) =>
{
    var shortenedUrl = await urlRepository.GetByCodeAsync(code);
    if (shortenedUrl is null) return Results.NotFound("Short link not found.");
    return Results.Redirect(shortenedUrl.OriginalUrl);
});

// --- Áp dụng Migrations ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
