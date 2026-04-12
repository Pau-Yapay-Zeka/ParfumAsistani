using Microsoft.EntityFrameworkCore;
using ParfumAsistani.Data;
using ParfumAsistani.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı bağlantımızı (SQL Server) projeye tanıtıyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. API Controller'larımızı aktifleştiriyoruz
builder.Services.AddControllers();

// 3. Swagger ve Yapay Zeka Servis Ayarları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ParfumAsistani.Services.IAiService, ParfumAsistani.Services.GeminiService>();

var app = builder.Build();

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