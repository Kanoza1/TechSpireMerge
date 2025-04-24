using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;
using PersonalLearning.ResponseModule;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Infrastructure.Data.Identity;


namespace PersonalLearning.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMailService mailService;
        private readonly IDentityUserDbContext context;
        private readonly IResponseCacheService cacheService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public IdentityController(SignInManager<AppUser> signInManager
            , UserManager<AppUser> userManager, IMapper mapper
            , ITokenService tokenService, IMailService mailService
            , IDentityUserDbContext context, IResponseCacheService cacheService, IUserService userService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.mailService = mailService;
            this.context = context;
            this.cacheService = cacheService;
            this.userService = userService;
        }
        [HttpPost("signup")]
        public async Task<ActionResult> Signup(SignupDto userVM)
        {
            if (ModelState.IsValid)
            {
                if (CheckEmailExist(userVM.Email).Result.Value)
                {
                    return BadRequest(new ApiException(400, "Email is already exists"));
                }
                var user = mapper.Map<AppUser>(userVM);
                user.UserName = user.FirstName + " " + user.LastName;
                var result = await userManager.CreateAsync(user, userVM.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                var code = mailService.GenerateCode();
                await cacheService.CacheResponseAsync(user.Email, code, TimeSpan.FromSeconds(3));
                await mailService.SendEmailAsync(user.Email, "", code);
                return Ok("You registered your account successfully, please verify the otp sent to your email");
            }
            return BadRequest(new ApiException(400, "Invalid Data, please try again"));
        }
        [HttpPost("VerifyOtp")]
        public async Task<ActionResult> VerifyOtp(string email, string otp)
        {
            var storedOtp = await cacheService.GetCachedResponse(email);
            if (storedOtp is null)
            {
                return BadRequest(new ApiException(400, "The Code is Expired, Do you want anothe code?"));
            }
            if (storedOtp == otp)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user is null)
                    return NotFound("user not found, please enter valid email");
                await cacheService.DeleteCachedResponse(email);
                user.IsVerified = true;
                await userManager.UpdateAsync(user);
                return Ok("Otp verified Successfully");
            }
            else
            {
                return BadRequest("The Otp is Wrong, Please try again!");
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto userVM)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<AppUser>(userVM);
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Email is required");
                }
                var appUser = await userManager.FindByEmailAsync(user.Email);
               
                if (appUser == null)
                {
                    return NotFound(Unauthorized(401));
                }
                var sucess = await userManager.CheckPasswordAsync(appUser, userVM.Password);
                if (!sucess)
                {
                    return NotFound(Unauthorized(401));
                }
                await signInManager.SignInAsync(appUser, false);
                var userApp = mapper.Map<UserDto>(appUser);
                userApp.Token = tokenService.CreateToken(appUser);

                return Ok(userApp);
            }
            return BadRequest(new ApiException(400, "InValid Data"));
        }
        /*[HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<AppUser> GetCurrentUser()
        => await userService.GetCurrentUser();*/
        [HttpGet("Logout")]
        [Authorize]
        public async Task Logout()
        => await signInManager.SignOutAsync();
        [HttpGet("CheckEmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
            => await userManager.FindByEmailAsync(email) != null;
        [HttpPost("SendCode")]
        public async Task<ActionResult> SendCode(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("Email Not exist, please enter valid email");
            }
            var code = mailService.GenerateCode();

            await cacheService.CacheResponseAsync(userEmail, code, TimeSpan.FromSeconds(3));
            await mailService.SendEmailAsync(userEmail, "", code);
            return Ok();
        }
        /* [HttpPost("ResetCode")]
         public async Task<ActionResult> ResetCode(string email,string code)
         {
             var storedCode=await cacheService.GetCachedResponse(email);
             if(storedCode is null)
             {
                 return BadRequest(new ApiException(400, "The Code is Expired, Do you want anothe code?"));
             }
             if(storedCode == code)
             {
                 await cacheService.DeleteCachedResponse(email);
                 return Ok("Code verified. You may proceed to reset your password.");
             }
             return BadRequest(new ApiException(400, "Invalid Code, please try again"));
         }*/
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            var isChanged = await userManager.ChangePasswordAsync(user, user.PasswordHash, password);
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
       /* [HttpPost("ForgetPassword")]
        [Authorize]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            if (!CheckEmailExist(email).Result.Value)
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
        }*/
        /*[HttpPost("ChangePasswordAfterVerification")]
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
        }*/
    }

}
