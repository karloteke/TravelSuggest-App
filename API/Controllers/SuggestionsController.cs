using Microsoft.AspNetCore.Mvc;
using TravelSuggest.Business;
using TravelSuggest.Models;
using Microsoft.AspNetCore.Authorization; 

namespace TravelSuggest.API.Controllers;

[ApiController]
[Route("[controller]")] 
// [Authorize]
public class SuggestionController : ControllerBase
{
    private readonly ILogger<SuggestionController> _logger;
    private readonly ISuggestionService _suggestionService;
    private readonly IUserService _userService;
    private readonly IDestinationService _destinationService;


    public SuggestionController(ILogger<SuggestionController> logger, ISuggestionService suggestionService, IUserService userService,  IDestinationService destinationService)
    {
        _logger = logger;
        _suggestionService = suggestionService;
        _destinationService = destinationService;
        _userService = userService;
    }

    [HttpGet(Name = "GetAllSuggestion")] 
    public ActionResult<IEnumerable<Suggestion>> GetAllSuggestion([FromQuery] SuggestionQueryParameters suggestionQueryParameters)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try 
        {
            var suggestion = _suggestionService.GetAllSuggestions(suggestionQueryParameters);
            
                if (suggestion == null || !suggestion.Any())
                    {
                        return NotFound("No hay sugerencias disponibles.");
                    }
                    
                    
            return Ok(suggestion);
        }     
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    [HttpGet("{suggestionId}", Name = "GetsuggestionById")]
    public IActionResult GetsuggestionById(int suggestionId)
    {
        try
        {
            var suggestion = _suggestionService.GetSuggestionById(suggestionId);
            return Ok(suggestion);     
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No existe sugerencias para el usuario con el Id {suggestionId}");
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpPost]
    public IActionResult Newsuggestion([FromBody] SuggestionCreateDTO suggestionDto, [FromQuery] int destinationId)
    {
        try 
        {
            // Verificar si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (suggestionDto.Price == null)
            {
                return BadRequest("El precio no puede ser nulo.");
            }

            // Verifica si el destino existe
            var destination = _destinationService.GetDestinationById(destinationId);
            if (destination == null)
            {
                return NotFound("El ID de destino no existe.");
            }

            // UserId del DTO
            _suggestionService.CreateSuggestion(
                suggestionDto.Title,
                suggestionDto.Description,
                suggestionDto.Price.Value,
                suggestionDto.Rating,
                DateTime.UtcNow,
                destinationId,
                suggestionDto.UserId,
                suggestionDto.ImageBase64
            );

            return Ok(new { message = $"Se ha creado correctamente la sugerencia para el destino con Id: {destinationId}" });

        }     
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpPut("{suggestionId}")]
    public IActionResult Updatesuggestion(int suggestionId, [FromBody] SuggestionUpdateDTO suggestionDto)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try
        {
            _suggestionService.UpdateSuggestionDetails(suggestionId, suggestionDto);
            return Ok(new { message = $"La sugerencia con Id: {suggestionId} ha sido actualizada correctamente"});
        }
        catch (KeyNotFoundException)
        {
           return NotFound();
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpDelete("{suggestionId}")]
    public IActionResult Deletesuggestion(int suggestionId)
    {
        try
        {
            _suggestionService.DeleteSuggestion(suggestionId);
             return Ok($"La sugerencia con Id: {suggestionId} ha sido borrada correctamente");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex.Message);
            return NotFound();
        }
    }   
}
