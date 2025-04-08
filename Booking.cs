using System;

namespace CoworkingBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int WorkspaceId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public User User { get; set; }
        public Workspace Workspace { get; set; }
    }
}
