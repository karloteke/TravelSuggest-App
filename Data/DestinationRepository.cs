using System.Globalization;
using System.Text.Json;
using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public class DestinationRepository :IDestinationRepository
    {
        private List<Destination> _destinations = new List<Destination>();
        private List<User> users = new List<User>();
        private readonly string _filePath;
        private readonly IUserRepository _userRepository;

         public DestinationRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _filePath = GetFilePath();
            LoadDestinations();
        }

        private string GetFilePath()
        {
            string basePath = Environment.GetEnvironmentVariable("IS_DOCKER") == "true" ? "SharedFolder" : "";
            return Path.Combine(basePath, "destinations.json");
        }

        public void AddDestination(Destination Destination)
        {
            //Verifico si existe el destino en la lista antes de agregarlo
            if(!_destinations.Any(d => d.Id == Destination.Id) )
            {
                _destinations.Add(Destination);
                SaveChanges();
            }
        }

        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? destinationQueryParameters)
        {
            var query = _destinations.AsQueryable();

            // Filtrar por nombre del destino
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.CityName))
            {
                query = query.Where(d => d.CityName != null && d.CityName.Contains(destinationQueryParameters.CityName));
            }

            // Filtrar por estación del año
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.Season))
            {
                query = query.Where(d => d.Season !=null && d.Season.Contains(destinationQueryParameters.Season));
            }

            // Filtrar por categoria
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.Category))
            {
                query = query.Where(d => d.Category !=null && d.Category.Contains(destinationQueryParameters.Category));
            }

            // Filtrar por UserId
            if (destinationQueryParameters?.UserId.HasValue == true)
            {
                query = query.Where(d => d.UserId == destinationQueryParameters.UserId.Value);
            }

            // Filtrar por popularidad
            if (destinationQueryParameters?.IsPopular.HasValue == true)
            {
                query = query.Where(d => d.IsPopular == destinationQueryParameters.IsPopular.Value);
            }

            return query.ToList();
        }

        public List<Destination> GetAllDestinations()
        {
            return _destinations;
        }

        public User? GetUserById(int? userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public Destination GetDestinationById(int DestinationId)
        {
            return _destinations.FirstOrDefault(d => d.Id == DestinationId);
        }

        public List<Destination> GetDestinations(int? userId)
        {
            // Implementación para obtener todas los destinos asociados a un usuario por su Id
            return _destinations.Where(du => du.UserId == userId).ToList();
        }

        public void DeleteDestination(int? DestinationId)
        {
            if (DestinationId != null)
            {
                var Destination = _destinations.FirstOrDefault(d => d.Id == DestinationId);
                if (Destination != null)
                {
                    _destinations.Remove(Destination);
                    SaveChanges();
                }
            }
        }

        public void UpdateDestination(Destination Destination)
        {
            AddDestination(Destination);
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_destinations, options);
            File.WriteAllText(_filePath, jsonString);
        }

        private void LoadDestinations()
        {
            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);
                var Destinations = JsonSerializer.Deserialize<List<Destination>>(jsonString);
                _destinations = Destinations ?? new List<Destination>();
            }

            if (_destinations.Any())
            {
                // int maxId = _Destinations.Max(a => a.Id);
                // Destination.UpdateNextDestinationId(maxId + 1);
            }
        }
    }
}
