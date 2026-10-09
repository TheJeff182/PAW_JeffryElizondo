using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await categoryRepository.ReadAsync() ?? [];
            return categories.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category == null)
                return NotFound();
            return CategoryDTO.ConvertFrom(category);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing CategoryId={categoryDTO.CategoryId}, Name={categoryDTO.Name}");

                bool isUpdate = categoryDTO.CategoryId > 0;

                if (isUpdate)
                {
                    // Para updates, recuperar la categoría existente primero
                    var existingCategory = await categoryRepository.FindAsync(categoryDTO.CategoryId);
                    if (existingCategory == null)
                    {
                        logger.LogWarning($"Save: Category with ID {categoryDTO.CategoryId} not found for update");
                        return false;
                    }

                    // Actualizar solo los campos que cambian
                    existingCategory.CategoryName = categoryDTO.Name;
                    existingCategory.Description = categoryDTO.Description;
                    existingCategory.ModifiedBy = categoryDTO.ModifiedBy;
                    existingCategory.LastModified = categoryDTO.ModifiedDate;

                    logger.LogInformation($"Save: Updating existing category - ID={existingCategory.CategoryId}, Name={existingCategory.CategoryName}");
                    await categoryRepository.UpdateAsync(existingCategory);
                }
                else
                {
                    // Para creación
                    var category = new Category
                    {
                        CategoryId = 0,
                        CategoryName = categoryDTO.Name,
                        Description = categoryDTO.Description,
                        ModifiedBy = categoryDTO.ModifiedBy,
                        LastModified = null
                    };

                    logger.LogInformation($"Save: Creating new category - Name={category.CategoryName}");
                    await categoryRepository.CreateAsync(category);
                }

                logger.LogInformation("Save: Category processed successfully");
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
            var category = await categoryRepository.FindAsync(id);
            if (category == null)
                return false;
            return await categoryRepository.DeleteAsync(category);
        }
    }
}

