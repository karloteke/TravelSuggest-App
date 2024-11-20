using TravelSuggest.Models;
using TravelSuggest.Data;


namespace TravelSuggest.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public User CreateUser(string userName, string password, string email)
        {
            var user = new User(userName, password, email);
        
            _repository.AddUser(user);
            _repository.SaveChanges();
            return user;
        }

        public List<User> GetAllUsers()
        {
            return _repository.GetUsers();
        }  

        public IEnumerable<User> GetAllUsers(UserQueryParameters? userQueryParameters)
        {
            return _repository.GetAllUsers(userQueryParameters);
        }


        public User? GetUserByUserName(string userName)
        {
            return _repository.GetUserByUserName(userName);
        }

        public User? GetUserByEmail(string userEmail)
        {
            return _repository.GetUserByEmail(userEmail);
        }

        public User? GetUserById(int id)
        {
            var user = _repository.GetUserById(id);

            if( user == null)
            {
                throw new KeyNotFoundException($"El usuario con Id {id} no existe.");
            }
            return  user;
        }

        public void UpdateUserDetails(int userId, UserUpdateDTO userUpdate)
        {
            // Obtener el usuario existente desde el repositorio
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"El usuario con Id: {userId} no existe.");
            }

            // Actualizar solo los campos proporcionados
            user.UserName = userUpdate.UserName;
            user.Email = userUpdate.Email;

            // Actualizar la contraseña solo si se proporciona
            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                user.Password = (userUpdate.Password); 
            }

            // Guardar los cambios
            _repository.UpdateUser(user);
            _repository.SaveChanges();
        }


        public void DeleteUser(int userId)
        {
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"El usuario con Id: {userId} no existe.");
            }
             _repository.DeleteUser(userId);         
        }

        public void SaveUser(User user)
        {
            _repository.UpdateUser(user); 
        }
    
        public User Authenticate(string userName, string password)
        {
           
            User? user = _repository.GetUserByUserName(userName);

           if (user != null &&  user.Password == password)
            {
                return user;
            }

            return null;
        }
    }
}