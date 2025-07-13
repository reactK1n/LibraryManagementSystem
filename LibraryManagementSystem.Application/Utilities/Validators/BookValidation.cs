using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Application.Utilities.Validators
{
    public class BookValidation
    {
        public class Isbn13OnlyAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var isbn = value as string;

                if (string.IsNullOrWhiteSpace(isbn))
                    return ValidationResult.Success; // Let [Required] handle null/empty if needed

                if (isbn.Length == 13 && isbn.All(char.IsDigit))
                    return ValidationResult.Success;

                return new ValidationResult("ISBN must be exactly 13 numeric digits.");
            }
        }
    }
}
