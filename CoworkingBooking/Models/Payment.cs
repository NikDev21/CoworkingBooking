namespace CoworkingBooking.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Success";
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Booking Booking { get; set; }
    }
}
