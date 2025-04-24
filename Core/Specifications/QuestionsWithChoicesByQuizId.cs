

using Core.Entities;
using PersonalLearning.Specifications;

namespace Core.Specifications
{
    public class QuestionWithChoicesByQuizId : BaseSpecification<Question>
    {
        public QuestionWithChoicesByQuizId(int quizId) : base(question => question.QuizId == quizId)
        {
           // AddInclude(question => question.Choices);
        }
    }
}
