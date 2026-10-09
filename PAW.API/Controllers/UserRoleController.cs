using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(ILogger<UserRoleController> logger, IUserRoleRepository userRoleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserRoles")]
        public async System.Threading.Tasks.Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var userRoles = await userRoleRepository.ReadAsync() ?? [];
            return userRoles.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:decimal}", Name = "GetUserRoleById")]
        public async System.Threading.Tasks.Task<ActionResult<UserRoleDTO>> GetById(decimal id)
        {
            var userRole = await userRoleRepository.FindByDecimalIdAsync(id);
            if (userRole == null)
                return NotFound($"UserRole with id {id} not found");

            return Ok(UserRoleDTO.ConvertFrom(userRole));
        }
    }
}
