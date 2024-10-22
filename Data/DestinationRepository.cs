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

        
        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? DestinationQueryParameters, bool orderByPopularAsc)
        {
            var query = _destinations.AsQueryable();


            if (!string.IsNullOrWhiteSpace(DestinationQueryParameters.CityName)) 
            {
            query = query.Where(d => d.CityName.Contains(DestinationQueryParameters.CityName));
            }

            if (!string.IsNullOrWhiteSpace(DestinationQueryParameters.Season)) 
            {
            query = query.Where(d => d.Season.Contains(DestinationQueryParameters.Season));
            }

            if (DestinationQueryParameters?.UserId.HasValue == true) // Verificamos si UserId tiene un valor
            {
                query = query.Where(d => d.UserId == DestinationQueryParameters.UserId.Value);
            }

            if (orderByPopularAsc) 
            {
                query = query.OrderByDescending(d => d.IsPopular);
            }
            

            var result = query.ToList();
            return result;
        }

        public IEnumerable<Destination> GetDestinationsForUser(DestinationQueryParameters? DestinationQueryParameters, bool orderByCategoryAsc)
        {
            var query = _destinations.AsQueryable();


            if (DestinationQueryParameters.UserId.HasValue && DestinationQueryParameters.UserId > 0)
            {
                query = query.Where(d => d.UserId == DestinationQueryParameters.UserId);
            }

            if (orderByCategoryAsc)
            {
                 query = query.OrderBy(d => d.Category);
            }
         
            var result = query.ToList();
            return result;
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
