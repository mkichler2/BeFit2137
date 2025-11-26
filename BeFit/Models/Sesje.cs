using System;
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
        [Display(Name = "Początek")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Koniec")]
        [DateRange("Start", ErrorMessage = "Czas zakończenia nie może być wcześniejszy niż czas rozpoczęcia.")]
        public DateTime End { get; set; }
    }

    // START <= KONIEC
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startProperty;

        public DateRangeAttribute(string startProperty)
        {
            _startProperty = startProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var end = (DateTime?)value;

            var startPropertyInfo = validationContext.ObjectType.GetProperty(_startProperty);
            if (startPropertyInfo == null)
                return new ValidationResult($"Unknown property: {_startProperty}");

            var startValue = (DateTime?)startPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (!startValue.HasValue || !end.HasValue)
                return ValidationResult.Success;

            if (startValue > end)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
