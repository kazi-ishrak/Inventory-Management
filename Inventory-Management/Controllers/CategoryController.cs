using Inventory_Management.Handler;
using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                var data = await _categoryService.GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return NoContent();
            }
        }
    }
}
