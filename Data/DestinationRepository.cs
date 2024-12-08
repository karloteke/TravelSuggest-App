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

        public void AddDestination(Destination destination, int userId)
        {
            // Verificar si existe el destino en la lista antes de agregarlo
            if (!_destinations.Any(d => d.Id == destination.Id))
            {
                _destinations.Add(destination);
                SaveChanges();

                var user = _userRepository.GetUserById(userId);
                if (user != null)
                {
                    user.AddPoints(150); // Asigna 150 puntos por crear un nuevo destino
                    _userRepository.SaveChanges(); 
                }
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

        public Destination GetDestinationById(int destinationId)
        {
            var destination = _destinations.FirstOrDefault(d => d.Id == destinationId);
            return destination;
        }

        public List<Destination> GetDestinations(int? userId)
        {
            // Implementación para obtener todas los destinos asociados a un usuario por su Id
            return _destinations.Where(du => du.UserId == userId).ToList();
        }

        public void DeleteDestination(int? destinationId)
        {
            if (destinationId != null)
            {
                var destination = _destinations.FirstOrDefault(d => d.Id == destinationId);
                if (destination != null)
                {
                    // Restar puntos al usuario
                    var user = _userRepository.GetUserById(destination.UserId); 
                    if (user != null)
                    {
                        _userRepository.SaveChanges(); 
                    }

                    _destinations.Remove(destination);
                    SaveChanges();
                }
            }
        }

        public void UpdateDestination(Destination destination, int userId)
        {
            AddDestination(destination, userId);
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_destinations, options);
            File.WriteAllText(_filePath, jsonString);
        }

        // private void LoadDestinations()
        // {
        //     if (File.Exists(_filePath))
        //     {
        //         string jsonString = File.ReadAllText(_filePath);
        //         var Destinations = JsonSerializer.Deserialize<List<Destination>>(jsonString);
        //         _destinations = Destinations ?? new List<Destination>();
        //     }

        //     if (_destinations.Any())
        //     {
        //         // int maxId = _Destinations.Max(a => a.Id);
        //         // Destination.UpdateNextDestinationId(maxId + 1);
        //     }
        // }

        private void LoadDestinations()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var loadedDestinations = JsonSerializer.Deserialize<List<Destination>>(jsonString);

                    _destinations = loadedDestinations ?? new List<Destination>();
                    Console.WriteLine($"Cargados {_destinations.Count} destinos desde {_filePath}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar destinos desde el archivo: {ex.Message}");
                    _destinations = new List<Destination>(); 
                }
            }
            else
            {
                Console.WriteLine($"El archivo {_filePath} no existe. Se inicializa una lista vacía de destinos.");
                _destinations = new List<Destination>();
            }

            if (_destinations.Any())
            {
                // Calcular el ID máximo
                // int maxId = _destinations.Max(a => a.Id);
                // Destination.UpdateNextDestinationId(maxId + 1);
            }
        }

    }
}
