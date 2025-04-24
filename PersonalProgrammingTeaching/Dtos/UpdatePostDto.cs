using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalLearning.Dtos
{
    public class UpdatePostDto
    {
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.Upload)]
        [FileSize(2 * 1024 * 1024, ErrorMessage = "Image1 must be less than 2MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only .jpg, .jpeg, and .png files are allowed.")]
        public IFormFile? Image1 { get; set; }

        [DataType(DataType.Upload)]
        [FileSize(2 * 1024 * 1024, ErrorMessage = "Image2 must be less than 2MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only .jpg, .jpeg, and .png files are allowed.")]
        public IFormFile? Image2 { get; set; }
    }
}
