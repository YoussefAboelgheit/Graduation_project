using System.ComponentModel.DataAnnotations;

namespace Test1._1.Custom_Attributes
{
    public class FileExtensionAttribute: ValidationAttribute
    {
        private readonly string[] _extensions;

        public FileExtensionAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return ValidationResult.Success; // Let [Required] handle this

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", _extensions)}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
