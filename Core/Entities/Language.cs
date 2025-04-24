

using Core.Entities.Core.Entities;

namespace Core.Entities
{
    public class Language:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AppUser> Users { get; set; }
            
    }
}
