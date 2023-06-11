using System.ComponentModel.DataAnnotations;

namespace Pronia.Attributes.ValidationAttributes
{
    public class AllowedFileTypesAttribute : ValidationAttribute
    {
        private readonly string[] _types;

        public AllowedFileTypesAttribute(params string[] types)
        {
            _types = types;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();


            if (value is IFormFile)
                files.Add((IFormFile)value);
            else if (value is List<IFormFile>)
                files = value as List<IFormFile>;

            foreach (var item in files)
            {
                if (!_types.Contains(item.ContentType))
                    return new ValidationResult("The file type must be one of these types: " + string.Join(",", _types));
            }

            return ValidationResult.Success;
        }
    }
}
