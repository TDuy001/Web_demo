using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        // Sử dụng enum để đại diện cho trạng thái đơn hàng 
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Thuộc tính chỉ đọc để hiển thị tóm tắt đơn hàng
        public string OrderSummary

        {
            get
            {
                return $"Order {OrderId} placed on {OrderDate.ToShortDateString()} with {Tickets.Count} tickets.";
            }
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // 1-n relationship: Customer -> Order
            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1-n relationship: Order -> Ticket
            builder.HasMany(o => o.Tickets)
                   .WithOne(t => t.Order)
                   .HasForeignKey(t => t.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
