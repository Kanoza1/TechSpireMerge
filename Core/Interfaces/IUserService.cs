using Core.Entities;


namespace Core.Interfaces
{
    public interface IUserService
    {
        public Task<AppUser> GetCurrentUser();
        public Task<Quiz> GetQuizByLevelId(int? levelId);
        public Task<List<Question>> GetQuestionsByQuizId(int quizId);

    }
}
