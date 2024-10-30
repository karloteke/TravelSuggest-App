using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public interface ISuggestionRepository
    {
        void AddSuggestion(Suggestion Suggestion, int UserId);
        List<Suggestion> GetAllSuggestions();
        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? SuggestionQueryParameters);
        Destination? GetDestinationById(int? destinationId);
        Suggestion GetSuggestionById(int? SuggestionId);
        List<Suggestion> GetSuggestions(int? destinationId);
        void UpdateSuggestion(Suggestion Suggestion, int userId);
        void DeleteSuggestion(int? id);
        void SaveChanges();
    }
}
