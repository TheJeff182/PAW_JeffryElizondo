using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserActionController(ILogger<UserActionController> logger, IUserActionRepository userActionRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserActions")]
        public async System.Threading.Tasks.Task<IEnumerable<UserActionDTO>> GetAll()
        {
            var userActions = await userActionRepository.ReadAsync() ?? [];
            return userActions.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id:decimal}", Name = "GetUserActionById")]
        public async System.Threading.Tasks.Task<ActionResult<UserActionDTO>> GetById(decimal id)
        {
            var userAction = await userActionRepository.FindByDecimalIdAsync(id);
            if (userAction == null)
                return NotFound($"UserAction with id {id} not found");

            return Ok(UserActionDTO.ConvertFrom(userAction));
        }
    }
}
