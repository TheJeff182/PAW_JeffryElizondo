using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger, IComponentRepository componentRepository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var components = await componentRepository.ReadAsync() ?? [];
            return components.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(int id)
        {
            var component = await componentRepository.FindAsync(id);
            return ComponentDTO.ConvertFrom(component);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Component> Components)
        {
            foreach (var c in Components)
            {
                if (c.Id > 0)
                    await componentRepository.CreateAsync(c);
                else 
                    await componentRepository.UpdateAsync(c);
            }

            return true;
        }
    }
}
