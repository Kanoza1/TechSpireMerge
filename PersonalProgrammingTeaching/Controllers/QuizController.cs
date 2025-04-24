

using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data.Identity;
using Infrastructure.Migrations;

//using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLearning.Dtos;
using PersonalLearning.Specifications;
using System.Security.Claims;
using Question = Core.Entities.Question;

namespace PersonalLearning.Controllers
{
    [Authorize]
    public class QuizController : BaseController
    {
       // private readonly IGenericRepository<QuestionsSolutions> questionSolutionRepository;
  

        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        public QuizController(IDentityUserDbContext context, IMapper mapper,
            UserManager<AppUser> userManager,
          
           IUnitOfWork unitOfWork,
           IUserService userService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
          
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }
       
        [HttpGet("GetQuestions")]
        public async Task<ActionResult> GetQuestions() {
            AppUser currentUser =await userService.GetCurrentUser();
            Quiz quiz = await userService.GetQuizByLevelId(currentUser.LevelId);
            Random rnd = new Random();
            List<Question> questions = quiz.Questions.OrderBy(org => rnd.Next()).Take(20).ToList();
            return Ok(mapper.Map<List<QuestionDto>>(questions));
        }
       [HttpPost("Answer")]
        public async Task<ActionResult> Answer(List<AnswerDto> answersDto)
        {
            List<QuestionsSolutions> answers=new List<QuestionsSolutions>();
            AppUser currentUser = await userService.GetCurrentUser();
            Quiz quiz = await userService.GetQuizByLevelId(currentUser.LevelId);
             
            answersDto.ForEach(answer =>
            {
                Question question= quiz.Questions.Find(q => q.Id == answer.QuestionId);
                QuestionsSolutions questionsSolutions = new QuestionsSolutions
                {
                    QuestionId=answer.QuestionId,
                    UserId = currentUser.Id,
                    QuizId = quiz.Id,
                    SolvedAt = answer.SolvedAt,
                    Answer = answer.Answer,
                    isCorrect = question.CorrectAnswer.CompareTo(answer.Answer) == 0
                };
                answers.Add(questionsSolutions);

            });
            QuizResult quizResult = new QuizResult
            {
                UserId = currentUser.Id,
                QuizId = quiz.Id,
                Score = answers.Where(answer => answer.isCorrect).Count()
            };
            await unitOfWork.Repository<QuestionsSolutions>().AddRange(answers);
          
            unitOfWork.Repository<QuizResult>().Add(quizResult);

            await unitOfWork.Complete();

            return Ok();
        }

       [HttpGet("GetResult")]
        public async Task<ActionResult> GetResult()
        {
            AppUser currentUser = await userService.GetCurrentUser();
            QuizResultByUserId quizResultSpec = new QuizResultByUserId(currentUser.Id);

            QuizResult quizResult = await unitOfWork.Repository<QuizResult>()
                .GetEntityWithSpecification(quizResultSpec);
            return Ok(quizResult.Score);
        }
        
    }
}
