using Microsoft.EntityFrameworkCore;
using ParfumAsistani.Models;

namespace ParfumAsistani.Data
{
    /// <summary>
    /// Uygulama veritabanı bağlamı. Clean Code ve açık isimlendirme prensipleriyle yazıldı.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Parfum> Parfumler { get; set; } = null!;
        public DbSet<Nota> Notalar { get; set; } = null!;
    }
}
