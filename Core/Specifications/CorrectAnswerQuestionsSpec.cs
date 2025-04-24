using Core.Entities;
using PersonalLearning.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class CorrectAnswerQuestionsSpec: BaseSpecification<QuestionsSolutions>
    {
        public CorrectAnswerQuestionsSpec() : base(qs=>qs.isCorrect)
        {
        }
    }
}
