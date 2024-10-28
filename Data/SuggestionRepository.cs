using System.Globalization;
using System.Text.Json;
using TravelSuggest.Models;
using static TravelSuggest.Models.Suggestion;

namespace TravelSuggest.Data
{
    public class SuggestionRepository :ISuggestionRepository
    {
        private List<Suggestion> _Suggestions = new List<Suggestion>();
        private List<User> _users = new List<User>();
        private List<Destination> _destinations = new List<Destination>();
        private readonly string _filePath;
        private readonly string _usersFilePath = "users.json"; 
        private readonly string _destinationsFilePath = "destinations.json"; 
        private readonly IDestinationRepository _destinationRepository;

         public SuggestionRepository(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository ?? throw new ArgumentNullException(nameof(destinationRepository));
            _filePath = GetFilePath();
            LoadData();
        }

        private string GetFilePath()
        {
            string basePath = Environment.GetEnvironmentVariable("IS_DOCKER") == "true" ? "SharedFolder" : "";
            return Path.Combine(basePath, "suggestions.json");
        }

        public void AddSuggestion(Suggestion suggestion)
        {
            if (!_Suggestions.Any(s => s.Id == suggestion.Id)) // Aseguramos de que no exista una sugerencia con el mismo ID
            {
                _Suggestions.Add(suggestion);
                SaveChanges(); // Guarda los cambios después de añadir la sugerencia
            }
        }
 
        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? suggestionQueryParameters)
        {
            var query = _Suggestions.AsQueryable();

            // Filtrar por DestinationId
            if (suggestionQueryParameters?.DestinationId.HasValue == true)
            {
                query = query.Where(s => s.DestinationId == suggestionQueryParameters.DestinationId.Value);
            }

            // Filtrar por rango de precios
            if (suggestionQueryParameters?.MinPrice.HasValue == true && suggestionQueryParameters?.MaxPrice.HasValue == true)
            {
                query = query.Where(s => s.Price >= suggestionQueryParameters.MinPrice.Value && s.Price <= suggestionQueryParameters.MaxPrice.Value);
            }
            else if (suggestionQueryParameters?.MinPrice.HasValue == true)
            {
                query = query.Where(s => s.Price >= suggestionQueryParameters.MinPrice.Value);
            }
            else if (suggestionQueryParameters?.MaxPrice.HasValue == true)
            {
                query = query.Where(s => s.Price <= suggestionQueryParameters.MaxPrice.Value);
            }

            // Filtrar por puntuación
            if (suggestionQueryParameters?.Rating.HasValue == true)
            {
                query = query.Where(s => s.Rating == suggestionQueryParameters.Rating.Value);
            }

             return query.ToList(); // Devuelve el resultado final
        }


        public List<Suggestion> GetAllSuggestions()
        {
            return _Suggestions;
        }

        public Destination? GetDestinationById(int? destinationId)
        {
            return _destinationRepository.GetDestinationById(destinationId.Value);
        }

        public Suggestion GetSuggestionById(int? SuggestionId)
        {
            return _Suggestions.FirstOrDefault(s => s.Id == SuggestionId);
        }

        public List<Suggestion> GetSuggestions(int? destinationId)
        {
            return _Suggestions.Where(d => d.DestinationId == destinationId).ToList();
        }

        public void DeleteSuggestion(int? SuggestionId)
        {
            if (SuggestionId != null)
            {
                var Suggestion = _Suggestions.FirstOrDefault(d => d.Id == SuggestionId);
                if (Suggestion != null)
                {
                    _Suggestions.Remove(Suggestion);
                    SaveChanges();
                }
            }
        }

        private void LoadData()
        {
            LoadUsers();
            LoadDestinations();
            LoadSuggestions();
        }

        private void LoadUsers()
        {
            if (File.Exists(_usersFilePath))
            {
                string jsonString = File.ReadAllText(_usersFilePath);
                _users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
            }
        }

        private void LoadDestinations()
        {
            if (File.Exists(_destinationsFilePath))
            {
                string jsonString = File.ReadAllText(_destinationsFilePath);
                _destinations = JsonSerializer.Deserialize<List<Destination>>(jsonString) ?? new List<Destination>();
            }
        }

        private void LoadSuggestions()
        {
            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);
                var suggestions = JsonSerializer.Deserialize<List<Suggestion>>(jsonString);
                _Suggestions = suggestions ?? new List<Suggestion>();

                // Asociar User y Destination para cada Suggestion
                foreach (var suggestion in _Suggestions)
                {
                    // Obtener el usuario por UserId y asignar Id y UserName
                    var user = _users.FirstOrDefault(u => u.Id == suggestion.UserId);
                    if (user != null)
                    {
                        suggestion.User = new UserPreview { UserName = user.UserName, Id = user.Id }; 
                    }

                    // Obtener el destino por DestinationId y asignarlo
                    suggestion.Destination = _destinations.FirstOrDefault(d => d.Id == suggestion.DestinationId);
                }
            }
        }

        public void UpdateSuggestion(Suggestion Suggestion)
        {
            AddSuggestion(Suggestion);
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_Suggestions, options);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
