
using Core.Entities.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public Level Level { get; set; }
        public int? LevelId { get; set; }
        public Language? Language { get; set; }
        public List<QuestionsSolutions> QuestionsSolutions { get; set; }

    }
}
