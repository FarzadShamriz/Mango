using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utilities
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if(file != null){
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Invalid file extension. Allowed extensions are: {string.Join(", ", _extensions)}.");
                }
            }

            return ValidationResult.Success;
        }

    }
}
