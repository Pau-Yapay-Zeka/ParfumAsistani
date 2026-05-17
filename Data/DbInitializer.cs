using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParfumAsistani.Models;

namespace ParfumAsistani.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Apply pending migrations for SQL Server; use EnsureCreated for SQLite fallback
            if (context.Database.IsSqlite())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }

            // Seed Notalar
            if (!context.Notalar.Any())
            {
                var notalar = new[]
                {
                    new Nota { Ad = "Vanilya", GorselUrl = "/mocks/vanilya.jpg", Aciklama = "Sıcak, tatlı ve kremamsı vanilya aroması." },
                    new Nota { Ad = "Gül", GorselUrl = "/mocks/gul.jpg", Aciklama = "Klasik ve zarif taze gül notası." },
                    new Nota { Ad = "Karamel", GorselUrl = "/mocks/karamel.jpg", Aciklama = "Tatlı, zengin karamel dokunuşu." },
                    new Nota { Ad = "Bergamot", GorselUrl = "/mocks/bergamot.jpg", Aciklama = "Ferah ve narenciye ferahlığı veren bergamot." },
                    new Nota { Ad = "Sandal Ağacı", GorselUrl = "/mocks/sandal.jpg", Aciklama = "Odunsu, kremamsı sandal ağacı karakteri." },
                    new Nota { Ad = "Lavanta", GorselUrl = "/mocks/lavanta.jpg", Aciklama = "Ferahnlatıcı ve aromatik lavanta notası." }
                };

                context.Notalar.AddRange(notalar);
                context.SaveChanges();
            }

            // Seed Parfumler
            if (!context.Parfumler.Any())
            {
                var parfums = new[]
                {
                    new Parfum { Ad = "Burberry Goddess", Marka = "Burberry" },
                    new Parfum { Ad = "Chanel No. 5", Marka = "Chanel" },
                    new Parfum { Ad = "Chanel Coco Mademoiselle", Marka = "Chanel" },
                    new Parfum { Ad = "Chanel Chance Eau Tendre", Marka = "Chanel" },
                    new Parfum { Ad = "Yves Saint Laurent Black Opium", Marka = "Yves Saint Laurent" },
                    new Parfum { Ad = "Yves Saint Laurent Libre", Marka = "Yves Saint Laurent" },
                    new Parfum { Ad = "Dior J'adore", Marka = "Dior" },
                    new Parfum { Ad = "Dior Miss Dior", Marka = "Dior" },
                    new Parfum { Ad = "Dior Hypnotic Poison", Marka = "Dior" },
                    new Parfum { Ad = "Lancôme La Vie Est Belle", Marka = "Lancôme" },
                    new Parfum { Ad = "Lancôme Idôle", Marka = "Lancôme" },
                    new Parfum { Ad = "Mugler Alien", Marka = "Mugler" },
                    new Parfum { Ad = "Mugler Angel", Marka = "Mugler" },
                    new Parfum { Ad = "Dior Sauvage", Marka = "Dior" },
                    new Parfum { Ad = "Chanel Bleu de Chanel", Marka = "Chanel" },
                    new Parfum { Ad = "Creed Aventus", Marka = "Creed" }
                };

                context.Parfumler.AddRange(parfums);
                context.SaveChanges();
            }
        }
    }
}
