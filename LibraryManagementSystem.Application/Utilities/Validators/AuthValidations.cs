using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Application.Utilities.Validators
{
    public class AuthValidations
    {
        public class StrongPasswordAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var password = value as string;

                if (string.IsNullOrWhiteSpace(password))
                    return new ValidationResult("Password is required");

                var hasUpper = new Regex(@"[A-Z]+");
                var hasLower = new Regex(@"[a-z]+");
                var hasDigit = new Regex(@"\d+");

                if (!hasUpper.IsMatch(password))
                    return new ValidationResult("Password must contain at least one uppercase letter.");
                if (!hasLower.IsMatch(password))
                    return new ValidationResult("Password must contain at least one lowercase letter.");
                if (!hasDigit.IsMatch(password))
                    return new ValidationResult("Password must contain at least one number.");
                if (password.Length < 8)
                    return new ValidationResult("Password must be at least 8 characters long.");

                return ValidationResult.Success;
            }
        }


        public class ValidEmailAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is not string email || string.IsNullOrWhiteSpace(email))
                    return new ValidationResult("Email is required");

                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email ? ValidationResult.Success : new ValidationResult("Invalid email format");
                }
                catch
                {
                    return new ValidationResult("Invalid email format");
                }
            }
        }


    }
}
