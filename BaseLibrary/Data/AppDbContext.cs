using BaseLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext:DbContext
    {
       
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<MovieDetail> MovieDetails { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(p => p.Details)
                .WithOne()
                .HasForeignKey<MovieDetail>(p => p.MovieId);

            modelBuilder.Entity<Review>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Review>()
           .HasOne(r => r.User)
           .WithMany(m => m.Reviews)
           .HasForeignKey(r => r.UserId);
        }
    }
}
