using System.Globalization;
using System.Text.Json;
using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public class SuggestionRepository :ISuggestionRepository
    {
        private List<Suggestion> _Suggestions = new List<Suggestion>();
        private List<User> users = new List<User>();
        private readonly string _filePath;
        private readonly IDestinationRepository _destinationRepository;

         public SuggestionRepository(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository ?? throw new ArgumentNullException(nameof(destinationRepository));
            _filePath = GetFilePath();
            LoadSuggestions();
        }

        private string GetFilePath()
        {
            string basePath = Environment.GetEnvironmentVariable("IS_DOCKER") == "true" ? "SharedFolder" : "";
            return Path.Combine(basePath, "suggestions.json");
        }

        public void AddSuggestion(Suggestion Suggestion)
        {
            //Verifico si existe el destino en la lista antes de agregarlo
            if(!_Suggestions.Any(d => d.Id == Suggestion.Id) )
            {
                _Suggestions.Add(Suggestion);
                SaveChanges();
            }
        }

        
        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? SuggestionQueryParameters, bool orderByPriceAsc)
        {
            var query = _Suggestions.AsQueryable();


            if (SuggestionQueryParameters?.DestinationId.HasValue == true) // Verificamos si DestinationId tiene un valor
            {
                query = query.Where(s => s.DestinationId == SuggestionQueryParameters.DestinationId.Value);
            }

            query = orderByPriceAsc ? query.OrderBy(s => s.Price) : query.OrderByDescending(s => s.Price);

            var result = query.ToList();
            return result;
        }

        public IEnumerable<Suggestion> GetSuggestionsForDestination(SuggestionQueryParameters? SuggestionQueryParameters, bool orderByRatingAsc)
        {
            var query = _Suggestions.AsQueryable();


            if (SuggestionQueryParameters.DestinationId.HasValue && SuggestionQueryParameters.DestinationId > 0)
            {
                query = query.Where(s => s.DestinationId == SuggestionQueryParameters.DestinationId);
            }

            if (orderByRatingAsc)
            {
                 query = query.OrderBy(s => s.Rating);
            }
         
            var result = query.ToList();
            return result;
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
            // Implementación para obtener todas los destinos asociados a un usuario por su Id
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

        private void LoadSuggestions()
        {
            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);
                var Suggestions = JsonSerializer.Deserialize<List<Suggestion>>(jsonString);
                _Suggestions = Suggestions ?? new List<Suggestion>();
            }

            if (_Suggestions.Any())
            {
                // int maxId = _Suggestions.Max(a => a.Id);
                // Suggestion.UpdateNextSuggestionId(maxId + 1);
            }
        }
    }
}
