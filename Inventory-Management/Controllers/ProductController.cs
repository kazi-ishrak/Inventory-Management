﻿using Inventory_Management.Handler;
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
        public async Task<IActionResult> GetAll()
        {
            string draw = Request.Form["draw"];
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string search = Request.Form["search[value]"];
            string sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string sortDirection = Request.Form["order[0][dir]"];

            var data = await _productService.GetAll();

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
            var data = await _productService.Create(input);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Product input)
        {
            
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
