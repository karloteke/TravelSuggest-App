using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public interface IDestinationRepository
    {
        void AddDestination(Destination destination, int userId);
        List<Destination> GetAllDestinations();
        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? DestinationQueryParameters);
        User? GetUserById(int? userId);
        Destination GetDestinationById(int DestinationId);
        List<Destination> GetDestinations(int? userId);
        void UpdateDestination(Destination destination, int userId);
        void DeleteDestination(int? id);
        void SaveChanges();
    }
}
