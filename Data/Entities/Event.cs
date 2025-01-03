using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public enum EventStatus
    {
        Scheduled,
        Ongoing,
        Completed,
        Cancelled
    }

    // Sửa đổi lớp Event thành lớp cơ sở
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Foreign keys
        [Required]
        public int VenueId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public byte[] RowVersion { get; set; }

        // Navigation properties
        public Venue Venue { get; set; }
        public Category Category { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();

        // Sử dụng enum để đại diện cho trạng thái sự kiện
        public EventStatus Status { get; set; } = EventStatus.Scheduled;

        // Thuộc tính chỉ đọc để hiển thị tóm tắt sự kiện
        public string EventSummary
        {
            get
            {
                return $"{Title} on {Date.ToShortDateString()} with {Tickets.Count} tickets.";
            }
        }
    }

    // Lớp con Concert kế thừa từ Event
    public class Concert : Event
    {
        public string Performer { get; set; }
    }

    // Lớp con Workshop kế thừa từ Event
    public class Workshop : Event
    {
        public string Topic { get; set; }
    }

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Cấu hình chiến lược kế thừa TPH
            builder.HasDiscriminator<string>("EventType")
                   .HasValue<Event>("Event")
                   .HasValue<Concert>("Concert")
                   .HasValue<Workshop>("Workshop");

            // 1-n relationship: Venue -> Event
            builder.HasOne(e => e.Venue)
                   .WithMany(v => v.Events)
                   .HasForeignKey(e => e.VenueId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1-n relationship: Category -> Event
            builder.HasOne(e => e.Category)
                   .WithMany(c => c.Events)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1-n relationship: Event -> Ticket
            builder.HasMany(e => e.Tickets)
                   .WithOne(t => t.Event)
                   .HasForeignKey(t => t.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            // n-n relationship: Event <-> Performer
            builder.HasMany(e => e.EventPerformers)
                   .WithOne(ep => ep.Event)
                   .HasForeignKey(ep => ep.EventId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
