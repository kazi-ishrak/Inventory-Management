using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            // Extract DataTable parameters
            string draw = Request.Form["draw"];
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string search = Request.Form["search[value]"];
            string sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string sortDirection = Request.Form["order[0][dir]"];

            // Fetch all data
            var data = await _auditLogService.GetAll();

            // Total records before filtering
            int recordsTotal = data.Count;

            // Apply search filtering
            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x =>
                    (x.ChangeType != null && x.ChangeType.ToLower().Contains(search.ToLower())) ||
                    (x.UserId.ToString().Contains(search)) ||
                    (x.ProductId.ToString().Contains(search)) ||
                    (x.Quantity.ToString().Contains(search))
                ).ToList();
            }

            // Total records after filtering
            int recordsFiltered = data.Count;

            // Apply dynamic sorting using reflection
            if (!string.IsNullOrEmpty(sortColumn))
            {
                var propertyInfo = typeof(AuditLog).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    data = sortDirection.ToLower() == "asc"
                        ? data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList()
                        : data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }

            // Apply pagination
            data = data.Skip(start).Take(length).ToList();

            // Return DataTable-compatible response
            return Ok(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            });
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _auditLogService.GetById(id);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(AuditLog input)
        {
            await _auditLogService.Create(input);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(AuditLog input)
        {
            await _auditLogService.Update(input);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _auditLogService.Delete(id);
            return Ok();
        }
    }
}
