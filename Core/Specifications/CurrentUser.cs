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
    public class CurrentUser : BaseSpecification<AppUser>
    {
        public CurrentUser(string id) : base(u => u.Id == id)
        {
        }
    }
}
