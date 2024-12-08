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


        public void CreateDestination(string cityName, string description, string season, bool isPopular, string category, int userId, string imageBase64)
        {

            var user = _repository.GetUserById(userId);

            if (user != null)
            { 
               
                var destination = new Destination(cityName, description, season, isPopular, category, userId, imageBase64);
           
                _repository.AddDestination(destination, userId);
           
                user.AddPoints(150);  // Asignar puntos al usuario
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

        public Destination GetDestinationById(int destinationId)
        {
            var destinations = _repository.GetAllDestinations();

            // Busca el destino por ID
            var destination = destinations.FirstOrDefault(d => d.Id == destinationId);
            
            if (destination == null)
            {
                throw new KeyNotFoundException($"El destino con ID {destinationId} no existe.");
            }
            
            return destination;
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

        public void UpdateDestinationDetails(int destinationId, DestinationUpdateDTO destinationUpdate)
        {
            var destination = _repository.GetDestinationById(destinationId);

            if (destination == null)
            {
                throw new KeyNotFoundException($"El destino con id: {destinationId} no existe.");
            }

            // Actualizar las propiedades del destino
            destination.CityName = destinationUpdate.CityName;
            destination.Description = destinationUpdate.Description;
            destination.Season = destinationUpdate.Season;
            destination.IsPopular = destinationUpdate.IsPopular ?? false;
            destination.Category = destinationUpdate.Category;
            destination.ImageBase64 = destinationUpdate.ImageBase64;
            _repository.UpdateDestination(destination, destination.UserId ?? 0);
            _repository.SaveChanges();
        }

        public void DeleteDestination(int destinationId)
        {
            // Obtener el destino
            var destination = _repository.GetDestinationById(destinationId);
            if (destination == null)
            {
                throw new KeyNotFoundException($"El destino con Id: {destinationId} no existe.");
            }

            // Obtener el usuario asociado
            var user = _repository.GetUserById(destination.UserId);
            if (user != null)
            {
                user.DeductPoints(150); // Restamos 150 puntos por eliminar el destino
            }

            _repository.DeleteDestination(destinationId);

            _repository.SaveChanges();
        }
    }
}