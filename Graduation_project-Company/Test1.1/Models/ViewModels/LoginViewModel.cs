using System.ComponentModel.DataAnnotations;

namespace Test1._1.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        
        public string HashedPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
