using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public interface IDestinationRepository
    {
        void AddDestination(Destination Destination);
        List<Destination> GetAllDestinations();
        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? DestinationQueryParameters);
        User? GetUserById(int? userId);
        Destination GetDestinationById(int DestinationId);
        List<Destination> GetDestinations(int? userId);
        void UpdateDestination(Destination Destination);
        void DeleteDestination(int? id);
        void SaveChanges();
    }
}
