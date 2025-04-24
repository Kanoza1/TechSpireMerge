

using Core.Entities.Core.Entities;

namespace Core.Entities
{
    public class Quiz:BaseEntity
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public int LevelId { get; set; }
        public Level Level { get; set; }
        public List<AppUser> Users { get; set; }
        public List<QuizResult> QuizResults { get; set; }

    }
}
