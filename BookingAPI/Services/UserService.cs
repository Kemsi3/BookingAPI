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



        //public User Get(UserLogin userLogin)
        //{
        //    User user = _context.Users.FirstOrDefault(o => o.Name.Equals(userLogin.Name, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userLogin.Password));

        //    return user;
        //}

    }
}
    

