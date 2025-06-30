using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Test1._1.Models.Entity;

namespace Test1._1.Custom_Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Email is required");
            }

            var email = value.ToString();
            var dbContext = (AppDBContext)validationContext.GetService(typeof(AppDBContext))!;

            // Check if email exists in User table (which covers both Applicant and Company)
            var emailExists = dbContext.Users.Any(u => u.Email == email && !u.IsDeleted);

            return emailExists
                ? new ValidationResult("This email is already exist")
                : ValidationResult.Success;
        }
    }
}