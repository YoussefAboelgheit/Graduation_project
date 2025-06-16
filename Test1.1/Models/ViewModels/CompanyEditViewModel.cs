using System.ComponentModel.DataAnnotations;

namespace Test1._1.Models.ViewModels
{
    public class CompanyEditViewModel
    {
        public int Id { get; set; }

        // All fields are optional for editing - no Required attributes
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FiledWork { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        // Optional password change - only applied if provided
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        // File uploads are optional for editing
        public IFormFile LogoFile { get; set; }
        public IFormFile TaxCardFile { get; set; }
        public IFormFile CommercialRegisterFile { get; set; }

        // Current file paths (for display purposes)
        public string CurrentLogo { get; set; }
        public string CurrentTaxCard { get; set; }
        public string CurrentCommercialRegister { get; set; }

        // Store original values to compare changes
        public string OriginalUserName { get; set; }
        public string OriginalPhoneNumber { get; set; }
        public string OriginalEmail { get; set; }
        public string OriginalFiledWork { get; set; }
        public string OriginalAddress { get; set; }
        public string OriginalDescription { get; set; }

        // Custom validation method
        public bool IsValidForUpdate()
        {
            // Phone validation - only if provided
            if (!string.IsNullOrWhiteSpace(Phone))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^01\d{9}$"))
                    return false;
            }

            // Email validation - only if provided
            if (!string.IsNullOrWhiteSpace(Email))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^.+@(gmail.com|hotmail.com)$"))
                    return false;
            }

            // Password validation - only if provided
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$"))
                    return false;

                // Check password confirmation
                if (Password != ConfirmPassword)
                    return false;
            }

            return true;
        }

        public Dictionary<string, string> GetValidationErrors()
        {
            var errors = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^01\d{9}$"))
                    errors.Add("Phone", "Phone must be exactly 11 numbers and start with 01");
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^.+@(gmail.com|hotmail.com)$"))
                    errors.Add("Email", "Please enter a valid Gmail or Hotmail email address");
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$"))
                    errors.Add("Password", "Password must be at least 8 characters long with at least: one uppercase letter, one lowercase letter, and one number.");

                if (Password != ConfirmPassword)
                    errors.Add("ConfirmPassword", "'Password' and 'Confirm password' do not match");
            }

            return errors;
        }
    }
}