using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int EventId { get; set; }

        public int? OrderId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public Order Order { get; set; }

        // Sử dụng enum để đại diện cho loại địa điểm 
        public VenueType Type { get; set; } = VenueType.ConcertHall;

        // Thuộc tính chỉ đọc để hiển thị tóm tắt vé 
        public string TicketSummary
        {
            get
            {
                return $"Ticket for {Event?.Title ?? "Unknown Event"} at ${Price}";
            }
        }
    }

    public abstract class TicketBase
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public decimal Price { get; set; }
        public byte[] RowVersion { get; set; } // Thêm thuộc tính RowVersion
        public Event Event { get; set; } // Thay đổi kiểu từ EventStatus sang Event
    }

    public class ConcertTicket : TicketBase
    {
        public string SeatNumber { get; set; }
        public VenueType TicketType { get; set; }
        public byte[] RowVersion { get; set; }

    }

    public class VipTicket : TicketBase
    {
        public VenueType TicketType { get; set; }

        public byte[] RowVersion { get; set; }
        public string LoungeAccess { get; set; }

    }

    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            // Cấu hình chiến lược kế thừa TPH 
            builder.HasDiscriminator<string>("TicketType")
                .HasValue<TicketBase>("Ticket")
                .HasValue<ConcertTicket>("ConcertTicket")
                .HasValue<VipTicket>("VipTicket");

            // 1-n relationship: Event -> Ticket
            builder.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n relationship: Order -> Ticket 
            builder.HasOne(t => t.Order)
                .WithMany(o => o.Tickets)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Setting the precision for the Price column 
            builder.Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
