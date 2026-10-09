using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository inventoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetInventories")]
        public async Task<IEnumerable<InventoryDTO>> GetAll()
        {
            var inventories = await inventoryRepository.ReadAsync() ?? [];
            return inventories.Select(InventoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory == null)
                return NotFound();
            return InventoryDTO.ConvertFrom(inventory);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] InventoryDTO inventoryDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing InventoryId={inventoryDTO.InventoryId}, UnitPrice={inventoryDTO.UnitPrice}");

                bool isUpdate = inventoryDTO.InventoryId > 0;

                if (isUpdate)
                {
                    var existingInventory = await inventoryRepository.FindAsync(inventoryDTO.InventoryId);
                    if (existingInventory == null)
                    {
                        logger.LogWarning($"Save: Inventory with ID {inventoryDTO.InventoryId} not found for update");
                        return false;
                    }

                    existingInventory.UnitPrice = inventoryDTO.UnitPrice;
                    existingInventory.UnitsInStock = inventoryDTO.UnitsInStock;
                    existingInventory.ProductId = inventoryDTO.ProductId;
                    existingInventory.ModifiedBy = inventoryDTO.ModifiedBy;
                    existingInventory.LastUpdated = DateTime.Now;

                    logger.LogInformation($"Save: Updating existing inventory - ID={existingInventory.InventoryId}");
                    await inventoryRepository.UpdateAsync(existingInventory);
                }
                else
                {
                    var inventory = new Inventory
                    {
                        UnitPrice = inventoryDTO.UnitPrice,
                        UnitsInStock = inventoryDTO.UnitsInStock,
                        ProductId = inventoryDTO.ProductId,
                        ModifiedBy = inventoryDTO.ModifiedBy,
                        DateAdded = DateTime.Now,
                        LastUpdated = DateTime.Now
                    };

                    logger.LogInformation($"Save: Creating new inventory");

                    await inventoryRepository.CreateAsync(inventory);
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
                logger.LogInformation($"Delete POST: InventoryId={id}");

                var inventory = await inventoryRepository.FindAsync(id);
                if (inventory == null)
                {
                    logger.LogWarning($"Delete: Inventory with ID {id} not found");
                    return false;
                }

                await inventoryRepository.DeleteAsync(inventory);
                logger.LogInformation($"Delete: Inventory {id} deleted successfully");
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
