namespace BookingAPI.Models
{
    public class Desk
    {
        public string DeskId { get; set; }
        
        public string? LocationId { get; set; }

        public bool IsAvailable { get; set; }
    }
}
