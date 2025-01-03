using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{

    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Required]
        public string Location { get; set; }
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();

        // Sử dụng enum để đại diện cho loại địa điểm 
        public VenueType Type { get; set; } = VenueType.ConcertHall;

        // Thuộc tính chỉ đọc để hiển thị tóm tắt địa điểm 
        public string VenueSummary
        {
            get
            {
                return $"{Name} located at {Location}, Type: {Type}";
            }
        }
    }

    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            // 1-n relationship: Venue -> Event
            builder.HasMany(v => v.Events)
                   .WithOne(e => e.Venue)
                   .HasForeignKey(e => e.VenueId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
