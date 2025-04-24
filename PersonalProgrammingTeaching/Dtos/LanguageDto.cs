using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalLearning.Dtos
{
    public class UserSelectionDto    
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public string Level { get; set; }
    }
}
