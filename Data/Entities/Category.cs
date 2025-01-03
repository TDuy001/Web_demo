using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();

        // Sử dụng enum để đại diện cho loại danh mục 
        public CategoryType Type { get; set; } = CategoryType.Music;
        // Thuộc tính chỉ đọc để hiển thị tóm tắt danh mục 
        public string CategorySummary
        {
            get
            {
                return $"{Name} (Type: {Type})";
            }
        }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // 1-n relationship: Category -> Event
            builder.HasMany(c => c.Events)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
