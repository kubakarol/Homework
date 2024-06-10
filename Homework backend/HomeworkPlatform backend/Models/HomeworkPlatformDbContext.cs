using Microsoft.EntityFrameworkCore;

namespace HomeworkPlatform_backend.Models
{
    public class HomeworkPlatformDbContext : DbContext
    {
        public HomeworkPlatformDbContext(DbContextOptions<HomeworkPlatformDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.Post)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.PostId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
