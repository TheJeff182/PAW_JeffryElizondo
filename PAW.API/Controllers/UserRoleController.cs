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

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async System.Threading.Tasks.Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var userRole = await userRoleRepository.FindAsync(id);
            return UserRoleDTO.ConvertFrom(userRole);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<bool> Save([FromBody] IEnumerable<UserRole> UserRoles)
        {
            foreach (var ur in UserRoles)
            {
                if (ur.Id > 0)
                    await userRoleRepository.CreateAsync(ur);
                else 
                    await userRoleRepository.UpdateAsync(ur);
            }

            return true;
        }
    }
}
