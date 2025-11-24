using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Mix
    {
        public int Id { get; set; }

        public int TypyId { get; set; }
        public virtual Typy? Typy { get; set; }

        public int SesjeId { get; set; }
        public virtual Sesje? Sesje { get; set; }

        [Required]
        public float Waga { get; set; }

        [Required]
        public int Serie { get; set; }

        [Required]
        public int Powtorzenia { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
