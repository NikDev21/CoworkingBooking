using System;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models;

namespace CoworkingBooking.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workspace>().HasData(
    new Workspace { Id = 4, Name = "Room C", Location = "First Floor", IsAvailable = true },
    new Workspace { Id = 5, Name = "Room D", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 6, Name = "Room E", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 7, Name = "Room F", Location = "Third Floor", IsAvailable = true },
    new Workspace { Id = 8, Name = "Room G", Location = "Third Floor", IsAvailable = true },
    new Workspace { Id = 9, Name = "Focus Booth 1", Location = "First Floor", IsAvailable = true },
    new Workspace { Id = 10, Name = "Focus Booth 2", Location = "First Floor", IsAvailable = true },
    new Workspace { Id = 11, Name = "Open Desk 1", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 12, Name = "Open Desk 2", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 13, Name = "Quiet Zone", Location = "Third Floor", IsAvailable = true },
    new Workspace { Id = 14, Name = "Hot Desk 1", Location = "Fourth Floor", IsAvailable = true },
    new Workspace { Id = 15, Name = "Hot Desk 2", Location = "Fourth Floor", IsAvailable = true },
    new Workspace { Id = 16, Name = "Hot Desk 3", Location = "Fourth Floor", IsAvailable = true },
    new Workspace { Id = 17, Name = "Solo Cabin 1", Location = "First Floor", IsAvailable = true },
    new Workspace { Id = 18, Name = "Solo Cabin 2", Location = "First Floor", IsAvailable = true },
    new Workspace { Id = 19, Name = "Team Table 1", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 20, Name = "Team Table 2", Location = "Second Floor", IsAvailable = true },
    new Workspace { Id = 21, Name = "Conference Spot", Location = "Third Floor", IsAvailable = true },
    new Workspace { Id = 22, Name = "Brainstorm Zone", Location = "Third Floor", IsAvailable = true },
    new Workspace { Id = 23, Name = "Meeting Box", Location = "Top Floor", IsAvailable = true });
        }
    }
}
