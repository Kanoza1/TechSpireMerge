using Core.Entities;
using PersonalLearning.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class QuestionById: BaseSpecification<Question>
    {
        public QuestionById(int id) : base(question => question.Id == id)
        {
        }
    }
}
