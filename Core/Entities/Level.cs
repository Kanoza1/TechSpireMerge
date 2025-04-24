

using Core.Entities.Core.Entities;

namespace Core.Entities
{
    public class Level:BaseEntity
    {
        public int Id { get; set; }
        public string level { get; set; }
        public List<AppUser> Users { get; set; }
        public Quiz Quiz { get; set; }
    }
  
}
