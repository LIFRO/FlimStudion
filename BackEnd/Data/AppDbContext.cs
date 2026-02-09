using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<FilmStudio> FilmStudios { get; set; } = null!;
    public DbSet<FilmCopy> FilmCopies { get; set; } = null!;
    public DbSet<User> Users  { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FilmStudio>()
            .HasMany(s => s.RentedFilms)
            .WithOne()
            .HasForeignKey(c => c.LoanedByFilmStudioId);
    }
    }
