using BookingAPI.Models;

namespace BookingAPI.Services
{
    public class UserService : IUserService
    {

        public readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public User Get(UserLogin userLogin)
        {
            User userFound = _context.Users.FirstOrDefault(user => user.Name == userLogin.Name && user.Password == userLogin.Password);

            return userFound;
        }

    }
}
    

