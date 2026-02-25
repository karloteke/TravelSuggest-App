using System.Collections.Generic; 
using TravelSuggest.Models; 

namespace TravelSuggest.Business
{
    public interface ISuggestionService
    {
        void CreateSuggestion(string title, string description, decimal price, int rating, DateTime created_at, int destinarionId, int userId, string? imageBase64 = null);  
        List<Suggestion> GetAllSuggestions();
        public Suggestion GetSuggestionById(int SuggestionId);
        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? SuggestionQueryParameters);
        List<Suggestion> GetSuggestions(int destinationId);
        public void UpdateSuggestionDetails(int SuggestionId, SuggestionUpdateDTO SuggestionUpdateDTO);
        public void DeleteSuggestion(int SuggestionId);
    }
}