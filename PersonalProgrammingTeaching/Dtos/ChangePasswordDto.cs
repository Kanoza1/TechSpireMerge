using System.ComponentModel.DataAnnotations;

namespace PersonalLearning.Dtos
{
    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
    }
}
