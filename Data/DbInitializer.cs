using Microsoft.EntityFrameworkCore;
using Demo3.Data.Entities;
using System;
using System.Linq;

namespace Demo3.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new CourseDbContext(serviceProvider.GetRequiredService<DbContextOptions<CourseDbContext>>()))
            {
                // Kiểm tra nếu bảng Courses đã có dữ liệu, nếu có thì không thêm dữ liệu mới
                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(
                        new Course
                        {
                            Title = "SVIP A",
                            Topic = "SEATING ",
                            ReleaseDate = DateTime.Today,
                            Author = "TICKET BOX "
                        },
                        new Course
                        {
                            Title = "SVIP B",
                            Topic = "SEATING",
                            ReleaseDate = DateTime.Today,
                            Author = "TICKET BOX "
                        },
                        new Course
                        {
                            Title = "VIP A",
                            Topic = "SEATING ",
                            ReleaseDate = DateTime.Today,
                            Author = "TICKET BOX "
                        },
                        new Course
                        {
                            Title = "VIP B",
                            Topic = "SEATING ",
                            ReleaseDate = DateTime.Today,
                            Author = "TICKET BOX "
                        },
                        new Course
                        {
                            Title = "FANZONE  ",
                            Topic = "STANDING ",
                            ReleaseDate = DateTime.Today,
                            Author = "TICKET BOX "
                        }
                    );
                }

                // Kiểm tra nếu bảng Event đã có dữ liệu, nếu có thì không thêm dữ liệu mới
                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                        new Event
                        {
                            EventId = 1,
                            Title = "Music Concert 2025",
                            Date = new DateTime(2025, 1, 20),
                            VenueId = 1, // My Dinh Stadium
                            CategoryId = 1, // Music
                            Status = EventStatus.Scheduled,
                            RowVersion = new byte[] { 0, 0, 0, 0 }
                        },
                        new Event
                        {
                            EventId = 2,
                            Title = "Tech Workshop",
                            Date = new DateTime(2025, 2, 15),
                            VenueId = 2, // National Theater
                            CategoryId = 2, // Workshop
                            Status = EventStatus.Scheduled,
                            RowVersion = new byte[] { 0, 0, 0, 1 }
                        },
                        new Event
                        {
                            EventId = 3,
                            Title = "Music Event 2025",
                            Date = new DateTime(2025, 1, 14),
                            VenueId = 1, // My Dinh Stadium
                            CategoryId = 1, // Music
                            Status = EventStatus.Scheduled,
                            RowVersion = new byte[] { 0, 0, 0, 2 }
                        }
                    );
                }

                if (!context.Venues.Any())
                {
                    context.Venues.AddRange(
                        new Venue
                        {
                            VenueId = 1,
                            Name = "Hanoi Concert Hall",
                            Location = "Hanoi, Vietnam",
                            Type = VenueType.ConcertHall,
                            RowVersion = new byte[] { 0, 0, 0, 0 }
                        },
                        new Venue
                        {
                            VenueId = 2,
                            Name = "Ho Chi Minh Stadium",
                            Location = "Ho Chi Minh City, Vietnam",
                            Type = VenueType.Stadium,
                            RowVersion = new byte[] { 0, 0, 0, 1 }
                        },
                        new Venue
                        {
                            VenueId = 3,
                            Name = "Da Nang Opera House",
                            Location = "Da Nang, Vietnam",
                            Type = VenueType.ConferenceCenter,
                            RowVersion = new byte[] { 0, 0, 0, 2 }
                        },
                        new Venue
                        {
                            VenueId = 4,
                            Name = "Hue Cultural Center",
                            Location = "Hue, Vietnam",
                            Type = VenueType.ConferenceCenter,
                            RowVersion = new byte[] { 0, 0, 0, 3 }
                        }
                    );
                }

                if (!context.Tickets.Any())
                {
                    context.Tickets.AddRange(
                    new Ticket
                    {
                        TicketId = 1,
                        EventId = 1,
                        OrderId = (int?)null,
                        Price = 50.00m,
                        Type = VenueType.ConcertHall,
                        RowVersion = new byte[] { 0, 0, 0, 0 }
                    },
                    new Ticket

                    {
                        TicketId = 2,
                        EventId = 1,
                        OrderId = 1,
                        Price = 100.00m,
                        Type = VenueType.ConcertHall,
                        RowVersion = new byte[] { 0, 0, 0, 1 }
                    },
                    new Ticket
                    {
                        TicketId = 3,
                        EventId = 2,
                        OrderId = (int?)null,
                        Price = 75.00m,
                        Type = VenueType.Stadium,
                        RowVersion = new byte[] { 0, 0, 0, 2 }
                    }
                    );
                }
                if (!context.ConcertTickets.Any())
                {
                    context.ConcertTickets.AddRange(
                        new ConcertTicket
                        {
                            TicketId = 4,
                            EventId = 1,
                            Price = 120.00m,
                            SeatNumber = "A1",
                            TicketType = VenueType.ConcertHall,
                            RowVersion = new byte[] { 0, 0, 0, 0 }
                        }
                    );
                }
                if (!context.VipTickets.Any())
                {
                    context.VipTickets.AddRange(
                        new VipTicket
                        {
                            TicketId = 5,
                            EventId = 2,
                            Price = 200.00m,
                            LoungeAccess = "Gold Lounge",
                            TicketType = VenueType.Stadium,
                            RowVersion = new byte[] { 0, 0, 0, 0 }
                        }
                    );
                }
                if (!context.Performers.Any())
                {
                    context.Performers.AddRange(
                        new Performer
                        {
                            PerformerId = 1,
                            Name = "John Doe",
                            Type = PerformerType.Singer
                        },
                        new Performer
                        {
                            PerformerId = 2,
                            Name = "Emily Davis",
                            Type = PerformerType.Dancer
                        },
                        new Performer
                        {
                            PerformerId = 3,
                            Name = "Michael Smith",
                            Type = PerformerType.Comedian
                        },
                        new Performer
                        {
                            PerformerId = 4,
                            Name = "Alice Brown",
                            Type = PerformerType.Musician
                        }
                    );
                }
                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(
                        new Order
                        {
                            OrderId = 1,
                            CustomerId = 1,
                            OrderDate = new DateTime(2025, 1, 1),
                            Status = OrderStatus.Pending,
                            RowVersion = new byte[] { 0, 0, 0, 0 }
                        },
                        new Order
                        {
                            OrderId = 2,
                            CustomerId = 2,
                            OrderDate = new DateTime(2025, 1, 2),
                            Status = OrderStatus.Completed,
                            RowVersion = new byte[] { 0, 0, 0, 1 }
                        },
                        new Order
                        {
                            OrderId = 3,
                            CustomerId = 3,
                            OrderDate = new DateTime(2025, 1, 3),
                            Status = OrderStatus.Cancelled,
                            RowVersion = new byte[] { 0, 0, 0, 2 }
                        }
                    );
                }
                if (!context.EventPerformers.Any())
                {
                    context.EventPerformers.AddRange(
                        new EventPerformer
                        {
                            EventId = 1,
                            PerformerId = 1,
                            Role = PerformerRole.MainAct
                        },
                        new EventPerformer
                        {
                            EventId = 1,
                            PerformerId = 2,
                            Role = PerformerRole.SupportAct
                        },
                        new EventPerformer
                        {
                            EventId = 2,
                            PerformerId = 3,
                            Role = PerformerRole.MainAct
                        },
                        new EventPerformer
                        {
                            EventId = 2,
                            PerformerId = 4,
                            Role = PerformerRole.GuestPerformer
                        }
                    );
                }
                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(
                        new Customer
                        {
                            CustomerId = 1,
                            Name = "John Doe",
                            Email = "johndoe@example.com",
                            Type = CustomerType.Regular
                        },
                        new Customer
                        {
                            CustomerId = 2,
                            Name = "Jane Smith",
                            Email = "janesmith@example.com",
                            Type = CustomerType.Premium
                        },
                        new Customer
                        {
                            CustomerId = 3,
                            Name = "Alice Johnson",
                            Email = "alice.johnson@example.com",
                            Type = CustomerType.Regular
                        },
                        new Customer
                        {
                            CustomerId = 4,
                            Name = "Bob Brown",
                            Email = "bob.brown@example.com",
                            Type = CustomerType.Guest
                        }
                    );
                }
                 if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category
                        {
                            CategoryId = 1,
                            Name = "Music",
                            Type = CategoryType.Music
                        },
                        new Category
                        {
                            CategoryId = 2,
                            Name = "Sports",
                            Type = CategoryType.Sports
                        },
                        new Category
                        {
                            CategoryId = 3,
                            Name = "Theater",
                            Type = CategoryType.Theater
                        },
                        new Category
                        {
                            CategoryId = 4,
                            Name = "OperaHouse",
                            Type = CategoryType.OperaHouse
                        }
                    );
                }



                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();
            }
        }
    }
}
