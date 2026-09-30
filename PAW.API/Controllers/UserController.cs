using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await userRepository.ReadAsync() ?? [];
            return users.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await userRepository.FindAsync(id);
            return UserDTO.ConvertFrom(user);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<User> Users)
        {
            foreach (var u in Users)
            {
                if (u.UserId > 0)
                    await userRepository.CreateAsync(u);
                else 
                    await userRepository.UpdateAsync(u);
            }

            return true;
        }
    }
}
