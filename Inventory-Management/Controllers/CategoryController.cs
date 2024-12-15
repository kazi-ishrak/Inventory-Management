using Inventory_Management.Handler;
using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
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

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            string draw = Request.Form["draw"];
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string search = Request.Form["search[value]"];
            string sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string sortDirection = Request.Form["order[0][dir]"];

            var data = await _categoryService.GetAll();

            int recordsTotal = data.Count;

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x =>
                (x.Name != null && x.Name.ToLower().Contains(search.ToLower()))
                ).ToList();
            }

            int recordsFiltered = data.Count;

            //Sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                data = data.AsQueryable().OrderBy(sortColumn + " " + sortDirection).ToList();
            }
            //Paging
            data = data.Skip(start).Take(length).ToList();
            return Ok(new { draw, recordsTotal, recordsFiltered, data = data });

            
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            try
            {
                var Data = await _categoryService.GetById(Id);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return NoContent();
            }
        }
    }
}
