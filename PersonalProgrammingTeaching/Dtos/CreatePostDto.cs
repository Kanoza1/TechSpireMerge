using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalLearning.Dtos
{
    public class CreatePostDto
    {
        /// <summary>
        /// The title of the post (required, max length: 100 characters).
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        public string Title { get; set; }

        /// <summary>
        /// The description of the post (required, max length: 500 characters).
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; }

        /// <summary>
        /// The first image file for the post (optional, max size: 2MB).
        /// </summary>
        [DataType(DataType.Upload)]
        [FileSize(2 * 1024 * 1024, ErrorMessage = "Image1 must be less than 2MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only .jpg, .jpeg, and .png files are allowed.")]
        public IFormFile Image1 { get; set; }

        /// <summary>
        /// The second image file for the post (optional, max size: 2MB).
        /// </summary>
        [DataType(DataType.Upload)]
        [FileSize(2 * 1024 * 1024, ErrorMessage = "Image2 must be less than 2MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only .jpg, .jpeg, and .png files are allowed.")]
        public IFormFile Image2 { get; set; }
    }

    /// <summary>
    /// Custom validation attribute for file size.
    /// </summary>
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > _maxSize)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Custom validation attribute for allowed file extensions.
    /// </summary>
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
