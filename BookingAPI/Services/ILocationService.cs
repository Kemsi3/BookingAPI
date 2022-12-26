using BookingAPI.Models;

namespace BookingAPI.IServices
{
    public interface ILocationProvider
    {
        Task<IResult> DeleteLocationById(string Locationid);

        Task<IResult> AddLocation(Location location);

        Task<IResult> GetAllLocations();
    }
}
