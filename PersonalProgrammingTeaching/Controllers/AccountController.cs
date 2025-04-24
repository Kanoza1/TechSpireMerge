using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;
using PersonalLearning.ResponseModule;

namespace PersonalLearning.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        // private readonly ITokenService tokenService;
        private readonly IMailService mailService;
        private readonly IResponseCacheService cacheService;
        private readonly IdentityController identityController;
        private readonly IMapper mapper;
        public AccountController(SignInManager<AppUser> signInManager,
            IdentityController identityController,
             IMapper mapper
            , ITokenService tokenService, IMailService mailService
            , IDentityUserDbContext context, IResponseCacheService cacheService)
        {
            this.identityController = identityController;
            this.mapper = mapper;
            this.mailService = mailService;
            this.cacheService = cacheService;
        }
        [HttpPost("ForgetPassword")]
        [Authorize]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            if (!identityController.CheckEmailExist(email).Result.Value)
            {
                return BadRequest(new ApiException(404, "Email you entered not registered, please go to signup"));
            }
            else
            {
                var code = mailService.GenerateCode();
                await cacheService.CacheResponseAsync(email, code, TimeSpan.FromSeconds(3));
                await mailService.SendEmailAsync(email, "Forget Password", code);
                return Ok($"We are automatically detecting a code\r\n sent to your email address {email}");
            }
        }
        [HttpPost("ChangePasswordAfterVerification")]
        [Authorize]
        public async Task<ActionResult> ChangePasswordAfterVerification(ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByEmailAsync(changePasswordDto.Email);
            var isChanged = await userManager.ChangePasswordAsync(user, user.PasswordHash, changePasswordDto.Password);
            if (isChanged.Succeeded)
            {
                return Ok("You changed your password successfully");
            }
            else
            {
                foreach (var item in isChanged.Errors)
                    return BadRequest(item);
            }
            return BadRequest();
        }
    }
}
