using TravelSuggest.Models;
using TravelSuggest.Data;

namespace TravelSuggest.Business
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _repository;

        public DestinationService(IDestinationRepository repository)
        {
            _repository = repository;
        }


        public void CreateDestination(string cityName, string description, string season, bool isPopular, string category, int userId)
        {

            var user = _repository.GetUserById(userId);

            if (user != null)
            { 
               
                var destination = new Destination(cityName, description, season, isPopular, category, userId);

                _repository.AddDestination(destination);
                _repository.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("No se pudo encontrar el destino con el ID proporcionado.");
            }
        }


        public List<Destination> GetAllDestinations()
        {
            return _repository.GetAllDestinations();
        }

        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? DestinationQueryParameters)
        {
            return _repository.GetAllDestinations(DestinationQueryParameters);
        }

        public User? GetUserById(int userId)
        {
            var User = _repository.GetUserById(userId);

            if(User == null)
            {
                throw new KeyNotFoundException($"El usuario con Id {userId} no existe.");
            }
            return User;
        }


        public Destination GetDestinationById(int DestinationId)
        {
            var Destination = _repository.GetDestinationById(DestinationId);
            
            if(Destination == null)
            {
                  throw new KeyNotFoundException($"El destino con Id {DestinationId} no existe.");
            }
            return Destination;
        }

        public List<Destination> GetDestinations(int userId)
        {
           // Obtener todas los destinos asociados a un usuario por su ID
            var Destinations = _repository.GetDestinations(userId);

            if (Destinations == null || Destinations.Count == 0)
            {
                throw new KeyNotFoundException($"No hay destinos para el usuario con ID: {userId}");
            }
            return Destinations;
        }

        public void UpdateDestinationDetails(int DestinationId, DestinationUpdateDTO DestinationUpdate)
        {
            var Destination = _repository.GetDestinationById(DestinationId);

            if (Destination == null)
            {
                throw new KeyNotFoundException($"El destino con id: {DestinationId} no existe.");
            }

            Destination.CityName = DestinationUpdate.CityName;
            Destination.Description = DestinationUpdate.Description;
            Destination.Season = DestinationUpdate.Season;
            Destination.IsPopular = DestinationUpdate.IsPopular ?? false;
            Destination.Category = DestinationUpdate.Category;
            _repository.UpdateDestination(Destination);
            _repository.SaveChanges();
        }

        public void DeleteDestination(int DestinationId)
        {
            var User = _repository.GetDestinationById(DestinationId);

            if (User == null)
            {
                throw new KeyNotFoundException($"El destino con Id: {DestinationId} no existe.");
            }
             _repository.DeleteDestination(DestinationId);         
        }
    }
}