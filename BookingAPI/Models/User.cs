namespace BookingAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }  

        public string Password { get; set; }

        public bool IsAdmin { get; set; }   

    }
}
