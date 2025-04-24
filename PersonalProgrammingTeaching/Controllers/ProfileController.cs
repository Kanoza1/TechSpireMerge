using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;
using System.Security.Claims;

namespace PersonalLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : BaseController
{
    private readonly IDentityUserDbContext _context;

    public ProfileController(IDentityUserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _context.users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return Unauthorized();

        var profileDto = new ProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return Ok(profileDto);
    }

    [HttpPut]
    [Authorize]

    public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        if (dto == null)
            return BadRequest();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _context.users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            return Unauthorized();

        user.UserName = dto.UserName ?? user.UserName;
        user.Email = dto.Email ?? user.Email;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;

        _context.SaveChanges();

        return NoContent();
    }
}
