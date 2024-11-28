using Microsoft.AspNetCore.Mvc;
using TravelSuggest.Business;
using TravelSuggest.Models;
using Microsoft.AspNetCore.Authorization; 

namespace TravelSuggest.API.Controllers;

[ApiController]
[Route("[controller]")] 
// [Authorize]
public class DestinationController : ControllerBase
{
    private readonly ILogger<DestinationController> _logger;
    private readonly IDestinationService _destinationService;
    private readonly IUserService _userService;

    public DestinationController(ILogger<DestinationController> logger, IDestinationService DestinationService, IUserService UserService)
    {
        _logger = logger;
        _destinationService = DestinationService;
        _userService = UserService;
    }

    [HttpGet(Name = "GetAllDestination")] 
    public ActionResult<IEnumerable<Destination>> GetAllDestination([FromQuery] DestinationQueryParameters DestinationQueryParameters)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try 
        {
            var Destination = _destinationService.GetAllDestinations(DestinationQueryParameters);
            
                if (Destination == null || !Destination.Any())
                    {
                        return NotFound("No hay destinos disponibles.");
                    }
                    
            return Ok(Destination);
        }     
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


    [HttpGet("{destinationId}", Name = " GetdestinationById")]
    public IActionResult  GetdestinationById(int destinationId)
    {
        try
        {
            var destination = _destinationService. GetDestinationById(destinationId);
            return Ok(destination);     
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No existe destinos para el usuario con el Id {destinationId}");
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpPost]
    public IActionResult NewDestination([FromBody] DestinationCreateDTO destinationDto, [FromQuery] int userId)
    {
        try 
        {
            // Verificar si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (destinationDto == null)
            {
                return BadRequest("El objeto DestinationCreateDTO no puede ser nulo.");
            }

            var user = _userService.GetUserById(userId);
            if(user == null)
            {
                return NotFound ("No existe ese ID");
            }

            _destinationService.CreateDestination(destinationDto.CityName, destinationDto.Description, destinationDto.Season, destinationDto.IsPopular ?? false, destinationDto.Category, userId);
            
            return Ok(new { message = $"Se ha creado correctamente el destino para el usuario con Id: {userId}" });
        }     
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpPut("{destinationId}")]
    public IActionResult Updatedestination(int destinationId, [FromBody] DestinationUpdateDTO destinationDto)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try
        {
            _destinationService.UpdateDestinationDetails(destinationId, destinationDto);
            return Ok($"El destino con Id: {destinationId} ha sido actualizado correctamente");
        }
        catch (KeyNotFoundException)
        {
           return NotFound();
        }
    }

    [Authorize(Roles = "admin, user")]
    [HttpDelete("{destinationId}")]
    public IActionResult DeleteDestination(int destinationId)
    {
        try
        {
            _destinationService.DeleteDestination(destinationId);
             return Ok($"El destino con Id: {destinationId} ha sido borrado correctamente");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex.Message);
            return NotFound();
        }
    }   
}
