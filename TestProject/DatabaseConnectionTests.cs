using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Data;

namespace CoworkingBooking.Tests
{
    public class DatabaseConnectionTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DatabaseConnectionTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql("Server=localhost;Port=3306;Database=CoworkingDB;User=root;Password=an2717ex",
                new MySqlServerVersion(new Version(8, 0, 32)))
                .Options;
        }

        [Fact]
        public void CanConnectToDatabase()
        {
            using var context = new AppDbContext(_options);

            try
            {
                var canConnect = context.Database.CanConnect();
                Assert.True(canConnect, "❌ Не удалось подключиться к базе данных.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ Ошибка подключения: {ex.Message}");
            }
        }
    }
}