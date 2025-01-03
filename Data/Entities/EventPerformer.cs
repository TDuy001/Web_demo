using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo3.Data.Entities
{
    public class EventPerformer
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int PerformerId { get; set; }
        public Performer Performer { get; set; }

        // Sử dụng enum để đại diện cho vai trò của nghệ sĩ biểu diễn 
        public PerformerRole Role { get; set; } = PerformerRole.MainAct;
        // Thuộc tính chỉ đọc để hiển thị tóm tắt màn biểu diễn 
        public string PerformanceSummary
        {
            get
            {
                return $"{Performer?.Name ?? "Unknown Performer"} performing as {Role} in {Event?.Title ?? "Unknown Event"}";
            }
        }
    }


    public class EventPerformerConfiguration : IEntityTypeConfiguration<EventPerformer>
    {
        public void Configure(EntityTypeBuilder<EventPerformer> builder)
        {
            // Composite primary key
            builder.HasKey(ep => new { ep.EventId, ep.PerformerId });

            // n-n relationship: Event <-> Performer
            builder.HasOne(ep => ep.Event)
                   .WithMany(e => e.EventPerformers)
                   .HasForeignKey(ep => ep.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ep => ep.Performer)
                   .WithMany(p => p.EventPerformers)
                   .HasForeignKey(ep => ep.PerformerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
