using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data.Identity;

public class IDentityUserDbContext : IdentityDbContext
{
    public IDentityUserDbContext(DbContextOptions<IDentityUserDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<AppUser> users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Book> Books { get; set; }

}
