using System.ComponentModel.DataAnnotations;

namespace ParfumAsistani.Models
{
    public class Parfum
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Ad { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Marka { get; set; } = null!;
    }
}
