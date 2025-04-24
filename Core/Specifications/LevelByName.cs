

using Core.Entities;
using PersonalLearning.Specifications;

namespace Core.Specifications
{
    public class LevelByName : BaseSpecification<Level>
    {
        public LevelByName(string type) : base(level => level.level == type)
        {
        }
    }
    
}
