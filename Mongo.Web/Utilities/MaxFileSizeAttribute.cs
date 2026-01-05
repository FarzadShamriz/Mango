using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utilities
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;
        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > _maxFileSize * 1024 * 1024)
                {
                    return new ValidationResult($"File size exceeds the maximum allowed size of {_maxFileSize} MB.");
                }
            }

            return ValidationResult.Success;
        }

    }
}
