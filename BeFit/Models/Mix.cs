using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Mix
    {
        public int Id { get; set; }

        [Display(Name = "Typ ćwiczeń")]
        public int TypyId { get; set; }
        public virtual Typy? Typy { get; set; }

        [Display(Name = "Sesja treningowa")]
        public int SesjeId { get; set; }
        public virtual Sesje? Sesje { get; set; }

        [Required]
        [Range(0, 10000)]
        [Display(Name = "Ciężar (kg)")]
        public float Waga { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Serie")]
        public int Serie { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Powtórzenia")]
        public int Powtorzenia { get; set; }

        [Required]
        [Display(Name = "Uzytkownik")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
