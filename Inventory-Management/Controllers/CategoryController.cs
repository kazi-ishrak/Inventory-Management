using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;
using static Inventory_Management.Models.DatabaseModel;

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
            var data = await _categoryService.GetAll();
            return Ok(data);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _categoryService.GetById(id);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Category input)
        {
            await _categoryService.Create(input);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Category input)
        {
            await _categoryService.Update(input);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}
