using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParfumAsistani.Services
{
    public interface IAiService
    {
        Task<string> GetOneriAsync(string ad, int yas, string cinsiyet, List<string> notalar, List<string> sevdigiParfumler);
    }
}
