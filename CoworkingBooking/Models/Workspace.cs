using System;
using System.ComponentModel.DataAnnotations;
namespace CoworkingBooking.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }
    }
}
