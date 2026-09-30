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

        [HttpGet("{id:int}", Name = "GetUserActionById")]
        public async System.Threading.Tasks.Task<ActionResult<UserActionDTO>> GetById(int id)
        {
            var userAction = await userActionRepository.FindAsync(id);
            return UserActionDTO.ConvertFrom(userAction);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<bool> Save([FromBody] IEnumerable<UserAction> UserActions)
        {
            foreach (var ua in UserActions)
            {
                if (ua.Id > 0)
                    await userActionRepository.CreateAsync(ua);
                else 
                    await userActionRepository.UpdateAsync(ua);
            }

            return true;
        }
    }
}
