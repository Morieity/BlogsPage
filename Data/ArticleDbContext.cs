using Microsoft.EntityFrameworkCore;
using ArticleAPI.Models;

namespace ArticleAPI.Data
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Content)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
