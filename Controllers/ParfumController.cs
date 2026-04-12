using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParfumAsistani.Data;
using ParfumAsistani.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParfumAsistani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParfumController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ParfumAsistani.Services.IAiService _aiService;

        public ParfumController(ApplicationDbContext db, ParfumAsistani.Services.IAiService aiService)
        {
            _db = db;
            _aiService = aiService;
        }

        [HttpGet("ara")]
        public async Task<ActionResult<IEnumerable<Parfum>>> Ara([FromQuery] string? kelime)
        {
            kelime ??= string.Empty;
            var lower = kelime.ToLower();

            var results = await _db.Parfumler
                .Where(p => p.Ad.ToLower().Contains(lower) || p.Marka.ToLower().Contains(lower))
                .OrderBy(p => p.Id)
                .Take(10)
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("notalar")]
        public async Task<ActionResult<IEnumerable<object>>> Notalar()
        {
            var list = await _db.Notalar
                .Select(n => new { n.Id, n.Ad, n.GorselUrl, n.Aciklama })
                .ToListAsync();

            return Ok(list);
        }

        public class OneriRequest
        {
            public string Ad { get; set; } = string.Empty;
            public int Yas { get; set; }
            public string Cinsiyet { get; set; } = string.Empty;
            public List<string> Notalar { get; set; } = new List<string>();
            public List<string> SevdigiParfumler { get; set; } = new List<string>();
        }

        [HttpPost("oneri")]
        public async Task<ActionResult> Oneri([FromBody] OneriRequest req)
        {
            var result = await _aiService.GetOneriAsync(req.Ad, req.Yas, req.Cinsiyet, req.Notalar, req.SevdigiParfumler);
            // Eğer servis zaten temiz bir JSON dizi döndürdüyse, doğrudan application/json olarak gönder.
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(result);
                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    return Content(result, "application/json");
                }
            }
            catch { /* result JSON değilse normal davranış */ }

            return Ok(new { öneri = result });
        }
    }
}
