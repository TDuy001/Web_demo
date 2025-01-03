using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class Performer
    {
        [Key]
        public int PerformerId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        // Navigation properties
        public ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();


        // Sử dụng enum để đại diện cho loại nghệ sĩ 
        public PerformerType Type { get; set; } = PerformerType.Singer;
        // Thuộc tính chỉ đọc để hiển thị tóm tắt nghệ sĩ
        public string PerformerSummary
        {
            get
            {
                return $"{Name}, a {Type}";
            }
        }
    }

    public class PerformerConfiguration : IEntityTypeConfiguration<Performer>
    {
        public void Configure(EntityTypeBuilder<Performer> builder)
        {
            // n-n relationship: Event <-> Performer
            builder.HasMany(p => p.EventPerformers)
                   .WithOne(ep => ep.Performer)
                   .HasForeignKey(ep => ep.PerformerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
