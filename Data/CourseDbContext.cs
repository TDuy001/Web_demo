using Microsoft.EntityFrameworkCore;
using Demo3.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace Demo3.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
    }

    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
}