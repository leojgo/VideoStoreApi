using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employee")]
    public class EmployeeidController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeeidController(EmployeeContext context)
        {
            _context = context;
        }

        //===========================================

        [HttpGet("{id}")]
        public IActionResult GetEmployeeInfoById(int id)
        {
            var newEmployeeUtils = new EmployeeUtils();
            var empToClean = newEmployeeUtils.GetEmployeeById(id);
            if (empToClean != null)
            {
                return Ok(empToClean);
            }

            return NotFound($"Couldnt find an Employee with the ID: {id}");
        }


        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] EmployeeInfoToShare updatedInfo, int id)
        {
            if (updatedInfo != null)
            {
                var newEmployeeUtils = new EmployeeUtils();
                if (updatedInfo.FirstName != null)
                {
                    updatedInfo.EmployeeId = id;
                    if (newEmployeeUtils.EditEmployeeAccount(updatedInfo))
                    {
                        return NoContent();
                    }
                }
            }
            return BadRequest("Invalid Input!!!");
        }
    }
}