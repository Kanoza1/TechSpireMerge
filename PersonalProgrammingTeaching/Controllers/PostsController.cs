using Core.Entities;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLearning.Dtos;
using PersonalLearning.ResponseModule;
using System.IO;

namespace PersonalLearning.Controllers;

/// <summary>
/// Controller for managing posts in the system.
/// </summary>
public class PostsController : BaseController
{
    private readonly IDentityUserDbContext _context;
    private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    private const string BaseUrl = "https://localhost:44338";

    public PostsController(IDentityUserDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all posts with full image URLs.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> Get()
    {
        var posts = await _context.Posts.ToListAsync();

        var result = posts.Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Image1 = !string.IsNullOrEmpty(post.Image1) ? $"{BaseUrl}{post.Image1}" : null,
            Image2 = !string.IsNullOrEmpty(post.Image2) ? $"{BaseUrl}{post.Image2}" : null,
            PublishDate = post.PublishDate
        });

        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific post by its ID with full image URLs.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetById(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the post you're looking for. Please check the ID and try again."));

        var result = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Image1 = !string.IsNullOrEmpty(post.Image1) ? $"{BaseUrl}{post.Image1}" : null,
            Image2 = !string.IsNullOrEmpty(post.Image2) ? $"{BaseUrl}{post.Image2}" : null,
            PublishDate = post.PublishDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Creates a new post with the provided details and uploads images.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromForm] CreatePostDto newPost)
    {
        if (string.IsNullOrEmpty(newPost.Title) || string.IsNullOrEmpty(newPost.Description))
            return BadRequest(new ApiResponse(400, "Title and Description are required to create a post."));

        var post = new Post
        {
            Title = newPost.Title,
            Description = newPost.Description,
            Image1 = newPost.Image1 != null ? await SaveImageAsync(newPost.Image1) : null,
            Image2 = newPost.Image2 != null ? await SaveImageAsync(newPost.Image2) : null,
            PublishDate = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var result = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Image1 = !string.IsNullOrEmpty(post.Image1) ? $"{BaseUrl}{post.Image1}" : null,
            Image2 = !string.IsNullOrEmpty(post.Image2) ? $"{BaseUrl}{post.Image2}" : null,
            PublishDate = post.PublishDate
        };

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, result);
    }

    /// <summary>
    /// Updates the details of an existing post.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdatePostDto updatedPost)
    {
        var existingPost = await _context.Posts.FindAsync(id);
        if (existingPost == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the post to update. Please check the ID and try again."));

        // Update Title only if a new value is provided
        if (!string.IsNullOrEmpty(updatedPost.Title))
            existingPost.Title = updatedPost.Title;

        // Update Description only if a new value is provided
        if (!string.IsNullOrEmpty(updatedPost.Description))
            existingPost.Description = updatedPost.Description;

        // Update Image1 only if a new image is provided
        if (updatedPost.Image1 != null)
            existingPost.Image1 = await SaveImageAsync(updatedPost.Image1);

        // Update Image2 only if a new image is provided
        if (updatedPost.Image2 != null)
            existingPost.Image2 = await SaveImageAsync(updatedPost.Image2);

        // Ensure PublishDate is updated to current date
        existingPost.PublishDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }


    /// <summary>
    /// Deletes a specific post by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the post to delete. Please check the ID and try again."));

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Saves an uploaded image to the server and returns its relative path.
    /// </summary>
    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension))
            throw new InvalidOperationException("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
        var filePath = Path.Combine(_imagePath, fileName);

        if (!Directory.Exists(_imagePath))
            Directory.CreateDirectory(_imagePath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"/images/{fileName}";
    }
}
