using Inventory_Management.Handler;
using Inventory_Management.Models;
using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using static Inventory_Management.Models.DatabaseModel;
namespace Inventory_Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromForm] DataTableRequestDTO request)
        {
            string searchValue =
                request.Search["value"] ?? "";

            string columnIndexString =
                request.Order.FirstOrDefault()?["column"] ?? "0";

            int columnIndex =
                int.Parse(columnIndexString);

            string sortColumn =
                request.Columns
                    .ElementAtOrDefault(columnIndex)?
                    ["data"] ?? "";

            string sortDirection =
                request.Order.FirstOrDefault()?["dir"] ?? "";

            var data = await _productService.GetAll();
            int recordsTotal = data.Count;

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x =>
                    (x.Name != null && x.Name.ToLower().Contains(searchValue.ToLower())) ||
                    (x.Sku != null && x.Sku.ToLower().Contains(searchValue.ToLower()))
                ).ToList();
            }

            int recordsFiltered = data.Count;

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                data = data.AsQueryable()
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToList();
            }

            data = data.Skip(request.Start).Take(request.Length).ToList();

            return Ok(new { request.Draw, recordsTotal, recordsFiltered, data });
        }



        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            try
            {
                var Data = await _productService.GetById(Id);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return NoContent();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product input)
        {
            await _productService.Create(input);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Product input)
        {
            input.Updated_at = DateTime.Now;
            await _productService.Update(input);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return Ok();
        }
    }
}
