using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Typy
    {
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Name { get; set; }
    }
}
