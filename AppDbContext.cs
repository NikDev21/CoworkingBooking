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
    }
}
