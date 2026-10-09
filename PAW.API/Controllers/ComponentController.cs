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

        [HttpGet("{id:decimal}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(decimal id)
        {
            var component = await componentRepository.FindByDecimalIdAsync(id);
            if (component == null)
                return NotFound();
            return ComponentDTO.ConvertFrom(component);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] ComponentDTO componentDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing ComponentId={componentDTO.ComponentId}, Name={componentDTO.Name}");

                bool isUpdate = componentDTO.ComponentId > 0;

                if (isUpdate)
                {
                    var existingComponent = await componentRepository.FindByDecimalIdAsync(componentDTO.ComponentId);
                    if (existingComponent == null)
                    {
                        logger.LogWarning($"Save: Component with ID {componentDTO.ComponentId} not found for update");
                        return false;
                    }

                    existingComponent.Name = componentDTO.Name;
                    existingComponent.Content = componentDTO.Content;

                    logger.LogInformation($"Save: Updating existing component - ID={existingComponent.Id}, Name={existingComponent.Name}");
                    await componentRepository.UpdateAsync(existingComponent);
                }
                else
                {
                    var component = new Component
                    {
                        Name = componentDTO.Name,
                        Content = componentDTO.Content
                    };

                    logger.LogInformation($"Save: Creating new component - Name={component.Name}");
                    await componentRepository.CreateAsync(component);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Save error: {ex.Message}");
                return false;
            }
        }

        [HttpDelete("{id:decimal}")]
        public async Task<bool> Delete(decimal id)
        {
            try
            {
                logger.LogInformation($"Delete POST: ComponentId={id}");

                var component = await componentRepository.FindByDecimalIdAsync(id);
                if (component == null)
                {
                    logger.LogWarning($"Delete: Component with ID {id} not found");
                    return false;
                }

                await componentRepository.DeleteAsync(component);
                logger.LogInformation($"Delete: Component {id} deleted successfully");
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
