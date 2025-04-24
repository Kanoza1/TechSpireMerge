
namespace Core.Entities
{
    public class QuizResult
    {
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public AppUser User { get; set; }
        public Quiz Quiz { get; set; }

    }
}
