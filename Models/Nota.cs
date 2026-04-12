using System.ComponentModel.DataAnnotations;

namespace ParfumAsistani.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Ad { get; set; } = null!;

        [MaxLength(1000)]
        public string? GorselUrl { get; set; }
       [MaxLength(500)]
        public string? Aciklama { get; set; }
    }
}
