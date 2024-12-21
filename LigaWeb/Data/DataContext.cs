using LigaWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LigaWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Game> Games { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>()
                .HasOne(g => g.HostClub)
                .WithMany(c => c.HostGames)
                .HasForeignKey(g => g.HostClubId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.VisitingClub)
                .WithMany(c => c.VisitingGames)
                .HasForeignKey(g => g.VisitingClubId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.Club)
                .WithMany()
                .HasForeignKey(u => u.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed for Roles and Users
            /* 
            string adminRoleId = Guid.NewGuid().ToString();
            string employeeRoleId = Guid.NewGuid().ToString();
            string clubRoleId = Guid.NewGuid().ToString();
            string anonimousRoleId = Guid.NewGuid().ToString();
            
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = employeeRoleId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = clubRoleId,
                    Name = "Club",
                    NormalizedName = "CLUB"
                },
                new IdentityRole
                {
                    Id = anonimousRoleId,
                    Name = "Anonimous",
                    NormalizedName = "ANONIMOUS"
                }
                );

            // Seed para Usuários
            var hasher = new PasswordHasher<IdentityUser>();

            string adminUserId = Guid.NewGuid().ToString();

            var adminUser = new User
            {
                Id = adminUserId,
                UserName = "fabiola.andrade.martins@outlook.com",
                NormalizedUserName = "FABIOLA.ANDRADE.MARTINS@OUTLOOK.COM",
                Email = "fabiola.andrade.martins@outlook.com",
                NormalizedEmail = "FABIOLA.ANDRADE.MARTINS@OUTLOOK.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456"),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            builder.Entity<IdentityUser>().HasData(adminUser);

            // Set Role to User
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId,
                }
                );*/
        }
    }
}

