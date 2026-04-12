using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParfumAsistani.Services
{
    public class GeminiService : IAiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<GeminiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetOneriAsync(string ad, int yas, string cinsiyet, List<string> notalar, List<string> sevdigiParfumler)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            // Google'ın en stabil ve güncel endpoint'i (gemini-flash-latest kullanılıyor)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={apiKey}";

                      var prompt = $@"
    Sen uzman bir parfüm danışmanısın.
    Kullanıcı: {ad}, {yas} yaşında, {cinsiyet}.
    Sevdiği Notalar: {string.Join(", ", notalar)}.
    Kullandığı Parfümler: {string.Join(", ", sevdigiParfumler)}.

    ÖNEMLİ: Sadece ve yalnızca geçerli bir JSON dizi (array) döndür. Hiçbir ek açıklama, başlık, madde işareti, Markdown veya düz metin ekleme — yanıt kesinlikle şu formatta olmalı:
    [
        {{ ""ad"": ""Parfüm Adı"", ""marka"": ""Markası"", ""neden"": ""Öneri sebebi..."", ""link"": ""https://www.google.com/search?tbm=shop&q=satın+al+Markası+Parfüm+Adı"" }},
        ...
    ]

    Her öğe tam olarak DÖRT anahtar içermelidir: ""ad"", ""marka"", ""neden"", ""link"". 
    ""link"" anahtarı için kullanıcının bu parfümü doğrudan satın alabileceği veya fiyatlarını görebileceği bir Google Alışveriş arama linki (q parametresi içine parfüm markası ve adı gelecek şekilde) oluştur. Anahtar isimleri ve JSON yapısı haricinde hiçbir şey yazma. Yanıtı sadece JSON dizisi olarak ver.

    ÖNEM: Her bir öneri birbirinden farklı ve çeşitli olsun; benzer ifadeler yerine üç farklı koku profili verin.
";

            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Google API Hatası: {StatusCode} - {Details}", response.StatusCode, responseString);
                    return $"API Hatası (Kod: {response.StatusCode}). Detaylar terminalde.";
                }

                using var document = JsonDocument.Parse(responseString);
                var text = document.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                    return "Öneri metni boş geldi.";

                // Beklenen çıktı: sadece JSON dizisi. Önce direkt parse etmeyi dene.
                try
                {
                    using var parsed = JsonDocument.Parse(text);
                    if (parsed.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        return parsed.RootElement.ToString();
                    }
                }
                catch { /* devam edelim; belki metin içinde JSON dizi var */ }

                // Eğer doğrudan JSON değilse, metin içinde ilk JSON dizisini cımbızla almaya çalış
                var start = text.IndexOf('[');
                var end = text.LastIndexOf(']');
                if (start >= 0 && end > start)
                {
                    var candidate = text.Substring(start, end - start + 1);
                    try
                    {
                        using var parsed2 = JsonDocument.Parse(candidate);
                        if (parsed2.RootElement.ValueKind == JsonValueKind.Array)
                        {
                            return parsed2.RootElement.ToString();
                        }
                    }
                    catch { }
                }

                _logger.LogWarning("Model yanıtı beklenen JSON dizi formatında değil; ham metin döndürülüyor.");
                return text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini REST isteğinde hata oluştu.");
                return $"Bağlantı hatası: {ex.Message}";
            }
        }
    }
}
