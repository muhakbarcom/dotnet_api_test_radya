using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        // public ApplicationDBContext(DbContextOptions options) : base(options) { }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // // Customize the ASP.NET Identity model and override the default table names
            // modelBuilder.Entity<User>(b =>
            //     {
            //         b.ToTable("Users");
            //     });

            // modelBuilder.Entity<IdentityRole>(b =>
            // {
            //     b.ToTable("Roles");
            // });

            // modelBuilder.Entity<IdentityUserRole<string>>(b =>
            // {
            //     b.ToTable("UserRoles");
            // });

            // modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            // {
            //     b.ToTable("UserClaims");
            // });

            // modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            // {
            //     b.ToTable("UserLogins");
            // });

            // modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            // {
            //     b.ToTable("RoleClaims");
            // });

            // modelBuilder.Entity<IdentityUserToken<string>>(b =>
            // {
            //     b.ToTable("UserTokens");
            // });


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user", NormalizedName = "CUSTOMER" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // seed user 1 Admin, 1 User
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "One",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Password123!"),
                    SecurityStamp = string.Empty
                },
                new User
                {
                    Id = "2",
                    UserName = "cust1",
                    FirstName = "Customer",
                    LastName = "One",
                    NormalizedUserName = "cust 1",
                    Email = "cust1@gmail.com",
                    NormalizedEmail = "cust1@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Password123!"),
                    SecurityStamp = string.Empty
                }
            );

            // seed user role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = roles[0].Id,
                    UserId = "1"
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles[1].Id,
                    UserId = "2"
                }
            );

            // Konfigurasi relasi OrderItem dengan Order dan Book
            modelBuilder.Entity<OrderItem>()
        .HasOne(oi => oi.Order)
        .WithMany(o => o.OrderItems)
        .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.BookId);

            // Konfigurasi relasi ShoppingCart dengan Book
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.Book)
                .WithMany(b => b.ShoppingCarts)
                .HasForeignKey(sc => sc.BookId);

            // Konfigurasi relasi ShoppingCart dengan User
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId);

            // Konfigurasi relasi Order dengan User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);


        }

    }
}