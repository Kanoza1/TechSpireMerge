using Core.Entities;
using PersonalLearning.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class QuizByLevelSpec : BaseSpecification<Quiz>
    {
        public QuizByLevelSpec(int? levelId) : base(quiz => quiz.LevelId==levelId)
        {

            AddInclude(q => q.Questions);

        }
    }
}
