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
            if (user == null)
                return NotFound($"User with id {id} not found");

            return Ok(UserDTO.ConvertFrom(user));
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = UserDTO.ConvertTo(userDTO);

                if (userDTO.UserId > 0)
                {
                    // Update
                    await userRepository.UpdateAsync(user);
                }
                else
                {
                    // Create
                    await userRepository.CreateAsync(user);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving user");
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                var user = await userRepository.FindAsync(id);
                if (user == null)
                    return false;

                await userRepository.DeleteAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting user with id {id}", id);
                return false;
            }
        }
    }
}
