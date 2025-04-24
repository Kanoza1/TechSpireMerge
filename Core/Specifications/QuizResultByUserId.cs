using Core.Entities;
using PersonalLearning.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class QuizResultByUserId : BaseSpecification<QuizResult>
    {
        public QuizResultByUserId(string userId) : base(qr=>qr.UserId==userId)
        {
        }
    }
}
