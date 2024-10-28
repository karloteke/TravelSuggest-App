using Microsoft.AspNetCore.Mvc;
using TravelSuggest.Business;
using TravelSuggest.Models;
using Microsoft.AspNetCore.Authorization; 

namespace TravelSuggest.API.Controllers;

[ApiController]
[Route("[controller]")] 
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService UserService)
    {
        _logger = logger;
        _userService = UserService;
    }
        
    [HttpGet(Name = "GetAllUsers")] 
    public ActionResult<IEnumerable<User>> GetAllUsers([FromQuery] UserQueryParameters userQueryParameters)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try 
        {
            var users = _userService.GetAllUsers(userQueryParameters);
            
                if (users == null || !users.Any())
                    {
                        return NotFound("No hay usuarios disponibles.");
                    }
                    
            return Ok(users);
        }     
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    
    [HttpGet("{userId}", Name = "GetUserById")]
    public IActionResult GetUser(int userId)
    {
        try
        {
            var user = _userService.GetUserById(userId);
            return Ok(user);
           
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No existe el usuario con el Id {userId}");
        }
    }

    [HttpPost]
    public IActionResult NewUser([FromBody] UserCreateDTO userDto)
    {
        try 
        {
            // Verificar si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest("Los campos no pueden estar vacíos.");
            }

            var userExist = _userService.GetUserByUserName(userDto.UserName);
            if (userExist != null)
            {
                return BadRequest("El usuario ya está registrado.");
            }

            var user = _userService.CreateUser(userDto.UserName, userDto.Password, userDto.Email);

            // Retornar la acción exitosa junto con el nuevo usuario creado
            return CreatedAtAction(nameof(GetAllUsers), new { userId = user.Id }, userDto);
        }     
        catch (Exception ex)
        {
            // Si ocurre un error, retornar un BadRequest con el mensaje de error
            return BadRequest(ex.Message);
        }
    }

    
    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDTO userDto)
    {
        if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

        try
        {
            _userService.UpdateUserDetails(userId, userDto);
            return Ok($"El usuario con Id: {userId} ha sido actualizado correctamente");
        }
        catch (KeyNotFoundException)
        {
           return NotFound();
        }
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        try
        {
            _userService.DeleteUser(userId);
            return Ok($"El usuario con Id: {userId} ha sido borrado correctamente");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex.Message);
            return NotFound($"El usuario con Id: {userId} no existe");
        }
    }
}
