using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext context;

        public PaymentsController(AppDbContext context)
        {
            this.context = context;
        }

        
        // POST /api/payments/pay
        [HttpPost("pay")]
        [Authorize]
        public async Task<IActionResult> SimulatePayment([FromBody] PaymentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await context.Bookings
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == dto.BookingId && b.UserID == userId);

            if (booking == null)
                return BadRequest("Бронирование не найдено");

            var payment = new Payment
            {
                BookingId = booking.Id,
                UserId = userId,
                Amount = dto.Amount,
                Status = "Success",
                PaidAt = DateTime.UtcNow
            };

            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            return Ok(payment);
        }
    }
}
