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
        base.OnModelCreating(modelBuilder);

        // Seed Events







        modelBuilder.ApplyConfiguration(new CourseConfiguration());

        modelBuilder.Entity<Event>()
        .HasOne(e => e.Venue)
        .WithMany(v => v.Events)
        .HasForeignKey(e => e.VenueId);


        modelBuilder.Entity<Event>()
        .HasOne(e => e.Category)
        .WithMany(c => c.Events)
        .HasForeignKey(e => e.CategoryId);

        modelBuilder.Entity<Ticket>()
        .HasOne(t => t.Event)
        .WithMany(e => e.Tickets)
        .HasForeignKey(t => t.EventId);

        modelBuilder.Entity<Ticket>()
        .HasOne(t => t.Order)
        .WithMany(o => o.Tickets)
        .HasForeignKey(t => t.OrderId);

        modelBuilder.Entity<Order>()
        .HasOne(o => o.Customer)
        .WithMany(c => c.Orders)
        .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<EventPerformer>()
        .HasKey(ep => new { ep.EventId, ep.PerformerId });

        modelBuilder.Entity<EventPerformer>()
        .HasOne(ep => ep.Event)
        .WithMany(e => e.EventPerformers)
        .HasForeignKey(ep => ep.EventId);

        modelBuilder.Entity<EventPerformer>()
        .HasOne(ep => ep.Performer)
        .WithMany(p => p.EventPerformers)
        .HasForeignKey(ep => ep.PerformerId);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>()
        .HasDiscriminator<string>("EventType")
        .HasValue<Event>("Event")
        .HasValue<Concert>("Concert")
        .HasValue<Workshop>("Workshop");

        modelBuilder.Entity<TicketBase>()
            .HasKey(t => t.TicketId);  

        modelBuilder.Entity<TicketBase>()
            .HasDiscriminator<string>("TicketType")
            .HasValue<TicketBase>("Ticket")
            .HasValue<ConcertTicket>("ConcertTicket")
            .HasValue<VipTicket>("VipTicket");



    }

    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<Event> Events { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Performer> Performers { get; set; }
    public DbSet<EventPerformer> EventPerformers { get; set; }
    public DbSet<ConcertTicket> ConcertTickets { get; set; }
    public DbSet<VipTicket> VipTickets { get; set; }
}