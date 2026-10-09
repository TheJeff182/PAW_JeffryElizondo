using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ILogger<ProductController> logger, IProductRepository productRepository) : ControllerBase
    {
        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await productRepository.ReadAsync() ?? [];
            return products.Select(ProductDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await productRepository.FindAsync(id);
            if (product == null)
                return NotFound();
            return ProductDTO.ConvertFrom(product);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] ProductDTO productDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing ProductId={productDTO.ProductId}, Name={productDTO.Name}");

                bool isUpdate = productDTO.ProductId > 0;

                if (isUpdate)
                {
                    // Para updates, recuperar el producto existente primero
                    var existingProduct = await productRepository.FindAsync(productDTO.ProductId);
                    if (existingProduct == null)
                    {
                        logger.LogWarning($"Save: Product with ID {productDTO.ProductId} not found for update");
                        return false;
                    }

                    // Actualizar solo los campos que cambian
                    existingProduct.ProductName = productDTO.Name;
                    existingProduct.Description = productDTO.Description;
                    existingProduct.Rating = productDTO.Rating;
                    existingProduct.ModifiedBy = productDTO.ModifiedBy;
                    existingProduct.LastModified = productDTO.ModifiedDate;

                    logger.LogInformation($"Save: Updating existing product - ID={existingProduct.ProductId}, Name={existingProduct.ProductName}");
                    await productRepository.UpdateAsync(existingProduct);
                }
                else
                {
                    // Para creación
                    var product = new Product
                    {
                        ProductId = 0,
                        ProductName = productDTO.Name,
                        Description = productDTO.Description,
                        Rating = productDTO.Rating,
                        ModifiedBy = productDTO.ModifiedBy,
                        CreatedBy = productDTO.CreatedBy,
                        LastModified = null
                    };

                    logger.LogInformation($"Save: Creating new product - Name={product.ProductName}");
                    await productRepository.CreateAsync(product);
                }

                logger.LogInformation("Save: Product processed successfully");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Save: Error occurred - {ex.Message}, Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var product = await productRepository.FindAsync(id);
            if (product == null)
                return false;
            return await productRepository.DeleteAsync(product);
        }
    }
}
