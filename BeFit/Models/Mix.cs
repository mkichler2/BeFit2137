using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Mix
    {
        public int Id { get; set; }

        [Display(Name = "Exercise Type")]
        public int TypyId { get; set; }
        public virtual Typy? Typy { get; set; }

        [Display(Name = "Training Session")]
        public int SesjeId { get; set; }
        public virtual Sesje? Sesje { get; set; }

        [Required]
        [Display(Name = "Weight (kg)")]
        public float Waga { get; set; }

        [Required]
        [Display(Name = "Series")]
        public int Serie { get; set; }

        [Required]
        [Display(Name = "Reps")]
        public int Powtorzenia { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
