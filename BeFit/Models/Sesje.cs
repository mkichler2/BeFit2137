using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class Sesje
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        [Required]
        [Display(Name = "Start treningu")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Koniec treningu")]
        public DateTime End { get; set; }
    }
}
