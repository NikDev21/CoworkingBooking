using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;

namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext context;
        public BookingsController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet] // Get all bookings
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await context.Bookings
                .Include(b => b.User)
                .Include(b => b.Workspace)
                .ToListAsync();
        }
        [HttpGet("{id}")] // Get booking by id
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();
            return booking;
        }
        [HttpPost] // Add(Create) new booking
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingCreateDto bookingDto)
        {
            if (bookingDto.StartDate < DateTime.Now || bookingDto.EndDate <= bookingDto.StartDate)
                return BadRequest("You cannot create booking in the past or with incorrect time.");

            var duration = bookingDto.EndDate - bookingDto.StartDate;
            if (duration.TotalMinutes < 30 || duration.TotalHours > 8)
                return BadRequest("Booking duration must be from 30 minutes till 8 hours.");

            bool hasConflict = await context.Bookings.AnyAsync(b =>
                b.WorkspaceId == bookingDto.WorkspaceId &&
                ((bookingDto.StartDate >= b.StartDate && bookingDto.StartDate < b.EndDate) ||
                 (bookingDto.EndDate > b.StartDate && bookingDto.EndDate <= b.EndDate) ||
                 (bookingDto.StartDate <= b.StartDate && bookingDto.EndDate >= b.EndDate)));

            if (hasConflict)
                return Conflict("This workspace is already booked for the selected time.");

            var booking = new Booking
            {
                UserID = bookingDto.UserID,
                WorkspaceId = bookingDto.WorkspaceId,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate
            };

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }
        [HttpPut("{id}")] // Update booking
        public async Task<IActionResult> UpdateBooking(int id, BookingUpdateDto bookingDto)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (bookingDto.StartDate != null)
                booking.StartDate = bookingDto.StartDate.Value;
            if (bookingDto.EndDate != null)
                booking.EndDate = bookingDto.EndDate.Value;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")] // Delete booking
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();
            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("{id}/cancel")] // Cancel booking
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();
            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
