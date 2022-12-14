
using Microsoft.AspNetCore.Mvc;

namespace LumartinRetoNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private List<User> usuarios;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            usuarios = new List<User>{
                new User("0504066358", "Luis"),
                new User("0102060408", "Andy"),
                new User("0907080603", "vero"),
                new User("1717007000", "Cesar Garcia"),
            };
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        [HttpGet("cedula")]
        public User GetUserByCedula(string cedula)
        {
            User? user = usuarios.Where(user => user.Cedula == cedula).SingleOrDefault();
            if (user == null) {
                _logger.LogError("||METODO GETUSERBYCEDULA||cedula:{}||CODIGO 500||");
                throw new ArgumentException($"El usuario con la cédula: {cedula} no existe");
            }else {
                string encodedId = Base64Encode(user.Cedula);
                User encodedUser = new User(encodedId, user.Nombre);
                _logger.LogInformation($"||METODO GETUSERBYCEDULA||cedula:{encodedId}||CODIGO 500||");
                return encodedUser;
            }
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            List<User> userEncodedList = new List<User>();
            foreach (User user in usuarios)
            {
                string encodedId = Base64Encode(user.Cedula);
                User userEncoded = new User(encodedId, user.Nombre);
                userEncodedList.Add(userEncoded);
                _logger.LogInformation($"||METODO GETUSERS||cedula:{encodedId}||CODIGO 200||");
                
            }
            return userEncodedList;
        }

    }
    public class User
    {

        public User(string cedula, string nombre)
        {
            Cedula = cedula;
            Nombre = nombre;
        }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
    }
}