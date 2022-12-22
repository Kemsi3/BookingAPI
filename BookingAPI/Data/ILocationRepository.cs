using BookingAPI.Models;

namespace BookingAPI.IServices
{
    public interface ILocationRepository
    {
        Task<IResult> DeleteLocationById(string Locationid);

        Task<IResult> AddLocation(Location location);

        Task<IResult> GetAllLocations();
    }
}
