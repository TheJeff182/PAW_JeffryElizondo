using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var suppliers = await supplierRepository.ReadAsync() ?? [];
            return suppliers.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier == null)
                return NotFound($"Supplier with id {id} not found");

            return Ok(SupplierDTO.ConvertFrom(supplier));
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] SupplierDTO supplierDTO)
        {
            try
            {
                var supplier = SupplierDTO.ConvertTo(supplierDTO);

                if (supplierDTO.SupplierId > 0)
                {
                    // Update
                    await supplierRepository.UpdateAsync(supplier);
                }
                else
                {
                    // Create
                    await supplierRepository.CreateAsync(supplier);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving supplier");
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                var supplier = await supplierRepository.FindAsync(id);
                if (supplier == null)
                    return false;

                await supplierRepository.DeleteAsync(supplier);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting supplier with id {id}", id);
                return false;
            }
        }
    }
}
