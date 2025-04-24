using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;
using PersonalLearning.ResponseModule;

namespace PersonalLearning.Controllers
{
    [Authorize]
    public class UserSelectionController : BaseController
    {
        private readonly IDentityUserDbContext context;
        private readonly IGenericRepository<Language> languageRepository;
        private readonly IGenericRepository<Level> levelRepository;


        public UserSelectionController(IDentityUserDbContext context,
            IGenericRepository<Language> languageRepository,
            IGenericRepository<Level> levelRepository)
        {
            this.context = context;
            this.languageRepository = languageRepository;
            this.levelRepository = levelRepository;
        }
        [HttpPost("Select")]
        public async Task<ActionResult> Select(UserSelectionDto selectionDto)
        {
            if(ModelState.IsValid)
            {
                LanguageByName languageByNameSpec = new LanguageByName(selectionDto.Language);
                LevelByName levelByNameSpec = new LevelByName(selectionDto.Level);
                Language language = await languageRepository.GetEntityWithSpecification(languageByNameSpec);
                Level level = await levelRepository.GetEntityWithSpecification(levelByNameSpec);
                AppUser user = context.Users.FirstOrDefault(u => u.Email == selectionDto.Email);
                user.Language = language;
                user.Level = level;
                await context.SaveChangesAsync();
                return Ok("We understand your interests, Let's Learn together");
            }              
                return BadRequest(new ApiException(401));
        }


}
}
