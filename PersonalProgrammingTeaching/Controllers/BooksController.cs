using Core.Entities;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalLearning.Dtos;

namespace PersonalLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : BaseController
{
    private readonly IDentityUserDbContext _context;

    public BooksController(IDentityUserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _context.Books
            .Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                BookUrl = book.BookUrl
            }).ToList();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            ImageUrl = book.ImageUrl,
            BookUrl = book.BookUrl
        };

        return Ok(bookDto);
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody] CreateBookDto dto)
    {
        if (dto == null)
            return BadRequest();

        var book = new Book
        {
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            BookUrl = dto.BookUrl
        };

        _context.Books.Add(book);
        _context.SaveChanges();

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            ImageUrl = book.ImageUrl,
            BookUrl = book.BookUrl
        };

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto dto)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();

        book.Title = dto.Title;
        book.Description = dto.Description;
        book.ImageUrl = dto.ImageUrl;
        book.BookUrl = dto.BookUrl;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        _context.SaveChanges();
        return NoContent();
    }
}
