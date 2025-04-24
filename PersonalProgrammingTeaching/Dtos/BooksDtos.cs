// Dtos/BookDto.cs
namespace PersonalLearning.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string BookUrl { get; set; }
    }

    public class CreateBookDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string BookUrl { get; set; }
    }

    public class UpdateBookDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string BookUrl { get; set; }
    }
}
