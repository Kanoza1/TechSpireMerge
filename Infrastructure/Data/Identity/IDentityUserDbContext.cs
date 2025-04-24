using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Infrastructure.Data.Identity
{
    public class IDentityUserDbContext:IdentityDbContext<AppUser>
    {
       public IDentityUserDbContext(DbContextOptions<IDentityUserDbContext>options)
            :base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
             builder
            .Entity<Level>()
            .Property(u => u.level)
            
            .HasConversion<string>();
           
            
            builder.Entity<AppUser>()
            .HasOne(u => u.Level)
          .WithMany(l => l.Users)
           .HasForeignKey(u => u.LevelId)
            .OnDelete(DeleteBehavior.SetNull);
            base.OnModelCreating(builder);
            builder.Entity<QuestionsSolutions>()
                .HasKey(qs => new { qs.UserId, qs.QuestionId });
            builder.Entity<QuizResult>()
                .HasKey(qr => new { qr.UserId, qr.QuizId });
            builder.Entity<Question>().HasOne(q => q.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<QuestionsSolutions> QuestionsSolutions {  get; set; }
        public DbSet<QuizResult> quizezResults { get;set; }
        public DbSet<Question> Questions { get; set; }




    }
}
