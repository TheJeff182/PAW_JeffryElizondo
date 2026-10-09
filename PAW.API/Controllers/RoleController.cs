using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await roleRepository.ReadAsync() ?? [];
            return roles.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role == null)
                return NotFound();
            return RoleDTO.ConvertFrom(role);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] RoleDTO roleDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing RoleId={roleDTO.RoleId}, RoleName={roleDTO.RoleName}");

                bool isUpdate = roleDTO.RoleId > 0;

                if (isUpdate)
                {
                    var existingRole = await roleRepository.FindAsync(roleDTO.RoleId);
                    if (existingRole == null)
                    {
                        logger.LogWarning($"Save: Role with ID {roleDTO.RoleId} not found for update");
                        return false;
                    }

                    existingRole.RoleName = roleDTO.RoleName;

                    logger.LogInformation($"Save: Updating existing role - ID={existingRole.RoleId}");
                    await roleRepository.UpdateAsync(existingRole);
                }
                else
                {
                    var role = new Role
                    {
                        RoleName = roleDTO.RoleName
                    };

                    logger.LogInformation($"Save: Creating new role");

                    await roleRepository.CreateAsync(role);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Save error: {ex.Message}");
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                logger.LogInformation($"Delete POST: RoleId={id}");

                var role = await roleRepository.FindAsync(id);
                if (role == null)
                {
                    logger.LogWarning($"Delete: Role with ID {id} not found");
                    return false;
                }

                await roleRepository.DeleteAsync(role);
                logger.LogInformation($"Delete: Role {id} deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Delete error: {ex.Message}");
                return false;
            }
        }
    }
}
