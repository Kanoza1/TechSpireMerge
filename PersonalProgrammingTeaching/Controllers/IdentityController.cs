using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;
using PersonalLearning.ResponseModule;
using System.Security.Claims;

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
        private readonly IMapper mapper;

        public IdentityController(SignInManager<AppUser> signInManager,
                                  UserManager<AppUser> userManager,
                                  IMapper mapper,
                                  ITokenService tokenService,
                                  IMailService mailService,
                                  IDentityUserDbContext context,
                                  IResponseCacheService cacheService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.mailService = mailService;
            this.context = context;
            this.cacheService = cacheService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Signup(SignupDto userVM)
        {
            if (await CheckEmailExist(userVM.Email))
                return BadRequest(new ApiException(400, "Email already exists"));

            var user = new AppUser
            {
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                Email = userVM.Email,
                PhoneNumber = userVM.PhoneNumber,
                UserName = userVM.Email.Split('@')[0]
            };

            var result = await userManager.CreateAsync(user, userVM.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var code = mailService.GenerateCode();
            await cacheService.CacheResponseAsync(user.Email, code, TimeSpan.FromMinutes(3));
            await mailService.SendEmailAsync(user.Email, "OTP Verification", code);

            return Ok("Registered successfully. Please verify the OTP sent to your email.");
        }

        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifyOtp(string email, string otp)
        {
            var storedOtp = await cacheService.GetCachedResponse(email);

            if (storedOtp is null)
                return BadRequest(new ApiException(400, "Code expired. Do you want another code?"));

            if (storedOtp != otp)
                return BadRequest(new ApiException(400, "Invalid OTP. Please try again."));

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            user.IsOtpVerified = true;
            await userManager.UpdateAsync(user);
            await cacheService.DeleteCachedResponse(email);

            return Ok("OTP verified successfully.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiException(400, "Invalid data"));

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var success = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!success)
                return Unauthorized("Invalid credentials");

            await signInManager.SignInAsync(user, false);

            var userDto = mapper.Map<UserDto>(user);
            userDto.Token = tokenService.CreateToken(user);

            return Ok(userDto);
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<AppUser>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            return Ok(user);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
            => Ok(await CheckEmailExist(email));

        private async Task<bool> CheckEmailExist(string email)
            => await userManager.FindByEmailAsync(email) != null;

        [HttpPost("send-code")]
        public async Task<ActionResult> SendCode(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return BadRequest("Email does not exist");

            var code = mailService.GenerateCode();
            await cacheService.CacheResponseAsync(userEmail, code, TimeSpan.FromMinutes(3));
            await mailService.SendEmailAsync(userEmail, "Verification Code", code);
            return Ok("Verification code sent.");
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(string email, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
                return Ok("Password changed successfully.");

            return BadRequest(result.Errors);
        }
    }
}
