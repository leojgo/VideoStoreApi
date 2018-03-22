using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employee")]
    public class EmployeeidController : Controller
    {
        private string _msg;

        private readonly EmployeeContext _context;

        public EmployeeidController(EmployeeContext context)
        {
            _context = context;
        }

        //===========================================

        [HttpGet("{id}")]
        public IActionResult GetEmployeeInfoById(int id)
        {
            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            EmployeeInfoToShare empToClean = newEmployeeUtils.GetEmployeeById(id);
            if (empToClean != null)
            {
                return Ok(empToClean);
            }

            _msg = $"Couldnt find an Employee with the ID: {id}";
            return NotFound(_msg);
        }


        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] EmployeeInfoToShare updatedInfo, int id)
        {
            if (updatedInfo != null)
            {
                EmployeeUtils newEmployeeUtils = new EmployeeUtils();
                if (updatedInfo.FirstName != null)
                {
                    updatedInfo.EmployeeId = id;
                    if (newEmployeeUtils.EditEmployeeAccount(updatedInfo, ref _msg))
                    {
                        return NoContent();
                    }
                }
            }

            _msg = "Invalid Input!!!";
            return BadRequest(_msg);
        }
    }
}