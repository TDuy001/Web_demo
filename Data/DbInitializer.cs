using Microsoft.EntityFrameworkCore;
using Demo3.Data.Entities;
using Microsoft.AspNetCore.Identity;


namespace Demo3.Data
{

    // public class DbInitializer
    // {
    //     private readonly CourseDbContext _context;
    //     private readonly UserManager<User> _userManager;
    //     private readonly RoleManager<IdentityRole> _roleManager;

    //     private readonly string AdminRoleName = "Admin";
    //     private readonly string UserRoleName = "Member";

    //     public DbInitializer(CourseDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    //     {
    //         _context = context;
    //         _userManager = userManager;
    //         _roleManager = roleManager;
    //     }

    //     public async Task Seed()
    //     {
    //         // Thêm vai trò
    //         if (!_roleManager.Roles.Any())
    //         {
    //             await _roleManager.CreateAsync(new IdentityRole { Name = AdminRoleName });
    //             await _roleManager.CreateAsync(new IdentityRole { Name = UserRoleName });
    //         }

    //         // Thêm người dùng
    //         if (!_userManager.Users.Any())
    //         {
    //             var admin = new User
    //             {
    //                 UserName = "admin@example.com",
    //                 Email = "admin@example.com",
    //                 FullName = "Admin User",
    //                 Dob = new DateTime(1990, 1, 1)
    //             };
    //             await _userManager.CreateAsync(admin, "Admin@123");
    //             await _userManager.AddToRoleAsync(admin, AdminRoleName);
    //         }
    //     }
    // }

    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new CourseDbContext(serviceProvider.GetRequiredService<DbContextOptions<CourseDbContext>>()))
            {
                // Look for any products.
                if (context.Courses.Any())
                {
                    return;   // DB has been seeded
                }
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
                context.SaveChanges();
            }
        }
    }
}