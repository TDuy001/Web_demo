using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Email { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // Sử dụng enum để đại diện cho loại khách hàng 
        public CustomerType Type { get; set; } = CustomerType.Regular;
        // Thuộc tính chỉ đọc để hiển thị tóm tắt khách hàng
        public string CustomerSummary
        {
            get
            {
                return $"{Name} ({Email}), Type: {Type}";
            }
        }
    }

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // 1-n relationship: Customer -> Order
            builder.HasMany(c => c.Orders)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
