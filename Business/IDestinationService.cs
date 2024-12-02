using System.Collections.Generic; 
using TravelSuggest.Models; 

namespace TravelSuggest.Business
{
    public interface IDestinationService
    {
        void CreateDestination(string CityName, string description, string season, bool isPopular, string category, int userId, string? imageBase64);  
        List<Destination> GetAllDestinations();
        public User GetUserById(int userId);
        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? DestinationQueryParameters);
        Destination GetDestinationById(int DestinationId);
        List<Destination> GetDestinations(int userId);
        public void UpdateDestinationDetails(int DestinationId, DestinationUpdateDTO DestinationUpdateDTO);
        public void DeleteDestination(int DestinationId);
    }
}