namespace PersonalLearning.Dtos.Article
{
    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IFormFile Image { get; set; }
        public double Rating { get; set; }
    }
}
