using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Demo3.Data.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }
    [StringLength(60)]
    [Required]
    public string? Topic { get; set; }
    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    [StringLength(60)]
    [Required]
    public string? Author { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(e => e.Lessons)
        .WithOne(e => e.Course)
        .HasForeignKey(e => e.CourseId)
        .HasPrincipalKey(e => e.Id);
    }
}