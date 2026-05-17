using System.IO;
using Microsoft.EntityFrameworkCore;
using ParfumAsistani.Data;
using ParfumAsistani.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı bağlantımızı projeye tanıtıyoruz (SQL Server veya SQLite fallback)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        var dbPath = Path.Combine(builder.Environment.ContentRootPath, "parfum.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
});

// 2. API Controller'larımızı aktifleştiriyoruz
builder.Services.AddControllers();

// 3. Swagger ve Yapay Zeka Servis Ayarları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ParfumAsistani.Services.IAiService, ParfumAsistani.Services.GeminiService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Seed(context);
}

// 4. Geliştirici ortamındaysak Swagger arayüzünü aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ================================================================
// ARAYÜZÜ GETİRECEK SİHİRLİ İKİ SATIR
// ================================================================
app.UseDefaultFiles(); // Tarayıcıya direkt girildiğinde index.html'i arayıp bulur.
app.UseStaticFiles();  // wwwroot klasörünü (CSS, JS, HTML) dış dünyaya açar.
// ================================================================

app.UseHttpsRedirection();
app.UseAuthorization();

// 5. Controller rotalarını haritala
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/frontend.html"));
app.Run();