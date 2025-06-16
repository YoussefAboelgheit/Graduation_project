using System.ComponentModel.DataAnnotations;
using Test1._1.Custom_Attributes;

namespace Test1._1.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string Lname { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "'Password' and 'Confirm password' do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Phone must be exactly 11 numbers and start with 01")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^.+@(gmail.com|hotmail.com)$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field of work is required")]
        public string Field_work { get; set; }

        [Required(ErrorMessage = "Years Experience of work is required")]
        public int Years_experience { get; set; }

        [Required(ErrorMessage = "CVFile is required")]
        [FileExtension(".pdf", ".jpg", ".jpeg", ".png")]
        public IFormFile CVFile { get; set; }

        [Required(ErrorMessage = "Logo is required")]
        [FileExtension(".jpg", ".jpeg", ".png")]
        public IFormFile Logo { get; set; }

        [Required(ErrorMessage = "Field of work is required")]
        public string FiledWork { get; set; }

        [Required(ErrorMessage = "Tax card is required")]
        [FileExtension(".pdf")]
        public IFormFile TaxCard { get; set; }

        [Required(ErrorMessage = "Commercial register is required")]
        [FileExtension(".pdf")]
        public IFormFile CommercialRegister { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Profile_image is required")]
        [FileExtension(".jpg", ".jpeg", ".png")]
        public IFormFile ProfileImage { get; set; }

    }

}
