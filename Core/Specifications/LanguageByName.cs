using Core.Entities;
using PersonalLearning.Specifications;


namespace Core.Specifications
{
    public class LanguageByName : BaseSpecification<Language>
    {
        public LanguageByName(string langName) 
            : base(lang=>lang.Name==langName)
        {
        }
    }
}
