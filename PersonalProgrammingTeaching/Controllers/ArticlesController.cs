using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data.Identity;
using PersonalLearning.Dtos.Article;
using PersonalLearning.ResponseModule; // For ApiResponse

namespace PersonalLearning.Controllers;

/// <summary>
/// Controller for managing articles.
/// </summary>
public class ArticlesController : BaseController
{
    private readonly IDentityUserDbContext _context;
    private readonly string _imagePath;
    private const string BaseUrl = "https://localhost:44338";

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesController"/> class.
    /// </summary>
    /// <param name="context">Database context for managing articles.</param>
    public ArticlesController(IDentityUserDbContext context)
    {
        _context = context;
        _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    }

    /// <summary>
    /// Retrieves all articles from the database.
    /// </summary>
    /// <returns>A list of articles with their details.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles()
    {
        var articles = await _context.Set<Article>().ToListAsync();

        if (!articles.Any())
            return NotFound(new ApiResponse(404, "No articles found."));

        var result = articles.Select(ToArticleDto);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific article by its ID.
    /// </summary>
    /// <param name="id">The ID of the article to retrieve.</param>
    /// <returns>The details of the requested article.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetArticle(int id)
    {
        var article = await _context.Set<Article>().FindAsync(id);

        if (article == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the article you're looking for. Please check the ID and try again."));

        return Ok(ToArticleDto(article));
    }

    /// <summary>
    /// Creates a new article in the database.
    /// </summary>
    /// <param name="articleDto">The details of the article to create.</param>
    /// <returns>The details of the created article.</returns>
    [HttpPost]
    public async Task<ActionResult<ArticleDto>> CreateArticle([FromForm] CreateArticleDto articleDto)
    {
        if (string.IsNullOrEmpty(articleDto.Title) || string.IsNullOrEmpty(articleDto.Description))
            return BadRequest(new ApiResponse(400, "Title and Description are required to create an article."));

        var article = new Article
        {
            Title = articleDto.Title,
            Description = articleDto.Description,
            Body = articleDto.Body,
            Image = articleDto.Image != null ? await SaveImageAsync(articleDto.Image) : null,
            Rating = articleDto.Rating
        };

        _context.Set<Article>().Add(article);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, ToArticleDto(article));
    }

    /// <summary>
    /// Updates an existing article by its ID.
    /// </summary>
    /// <param name="id">The ID of the article to update.</param>
    /// <param name="articleDto">The new details of the article.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArticle(int id, [FromForm] UpdateArticleDto articleDto)
    {
        var existingArticle = await _context.Set<Article>().FindAsync(id);

        if (existingArticle == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the article to update. Please check the ID and try again."));

        UpdateArticleFromDto(existingArticle, articleDto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing article by its ID.
    /// </summary>
    /// <param name="id">The ID of the article to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var article = await _context.Set<Article>().FindAsync(id);

        if (article == null)
            return NotFound(new ApiResponse(404, "Oops! We couldn't find the article to delete. Please check the ID and try again."));

        _context.Set<Article>().Remove(article);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Converts an Article entity to an ArticleDto object.
    /// </summary>
    /// <param name="article">The article entity to convert.</param>
    /// <returns>The converted ArticleDto object.</returns>
    private ArticleDto ToArticleDto(Article article) =>
        new()
        {
            Id = article.Id,
            Title = article.Title,
            Description = article.Description,
            Body = article.Body,
            ImageUrl = !string.IsNullOrEmpty(article.Image) ? $"{BaseUrl}{article.Image}" : null,
            Rating = article.Rating
        };

    /// <summary>
    /// Updates an Article entity using data from an UpdateArticleDto object.
    /// </summary>
    /// <param name="article">The article entity to update.</param>
    /// <param name="dto">The data transfer object containing updated data.</param>
    private void UpdateArticleFromDto(Article article, UpdateArticleDto dto)
    {
        article.Title = dto.Title ?? article.Title;
        article.Description = dto.Description ?? article.Description;
        article.Body = dto.Body ?? article.Body;

        if (dto.Image != null)
            article.Image = SaveImageAsync(dto.Image).GetAwaiter().GetResult();

        if (dto.Rating.HasValue)
            article.Rating = dto.Rating.Value;
    }

    /// <summary>
    /// Saves an uploaded image to the server and returns its relative path.
    /// </summary>
    /// <param name="imageFile">The image file to save.</param>
    /// <returns>The relative path of the saved image.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the file type is not allowed.</exception>
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

        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return $"/images/{fileName}";
    }
}
