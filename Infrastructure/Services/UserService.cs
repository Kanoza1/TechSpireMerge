using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
//using Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;



namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
       // private readonly IGenericRepository<AppUser> userRepository;
       // private readonly IGenericRepository<Quiz> quizRepository;
       // private readonly IGenericRepository<Core.Entities.Question> questionRepository;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserService(
            IGenericRepository<AppUser> userRepository,
            IGenericRepository<Quiz> quizRepository,
            IGenericRepository<Core.Entities.Question> questionRepository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork
        )
        {
           // this.userRepository = userRepository;
           // this.quizRepository = quizRepository;
           // this.questionRepository = questionRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }
        public async Task<AppUser> GetCurrentUser()
        {
            string userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            CurrentUser currentUserSpec = new CurrentUser(userId);
            return await unitOfWork.Repository<AppUser>().GetByIdAsync(userId);
        }

       

        public async Task<Quiz> GetQuizByLevelId(int? levelId)
        {
            QuizByLevelSpec quizByLevelSpec = new QuizByLevelSpec(levelId);
            return await unitOfWork.Repository<Quiz>().GetEntityWithSpecification(quizByLevelSpec);
        }

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
        {
            QuestionWithChoicesByQuizId questionWithChoicesSpec = new QuestionWithChoicesByQuizId(quizId);
            return await unitOfWork.Repository<Question>().ListAsync(questionWithChoicesSpec);
        }
    }
}
