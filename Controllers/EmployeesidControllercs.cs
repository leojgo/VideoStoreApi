using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employee")]
    public class EmployeesidController : Controller
    {
        private string _msg;

        private readonly EmployeeContext _context;

        public EmployeesidController(EmployeeContext context)
        {
            _context = context;
        }

        //===========================================

        [HttpGet("{id}")]
        public Employee GetEmployeeInfoById(int id)
        {
            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            return newEmployeeUtils.GetEmployeeById(id);
        }


        [HttpPost]
        public IActionResult UpdateInfo([FromBody] Employee updatedInfo)  //// THIS DOESNT MAP RIGHT!!
        {
            if (updatedInfo != null)
            {
                EmployeeUtils newEmployeeUtils = new EmployeeUtils();
                if (updatedInfo.FirstName != null)
                {
                    if (newEmployeeUtils.EditEmployeeAccount(updatedInfo, ref _msg))
                    {
                        return NoContent();
                    }
                }
            }

            _msg = "Invalid Input!!!";
            return BadRequest(_msg);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            if (newEmployeeUtils.MakeEmployeeInactive(id, ref _msg))
            {
                return NoContent();
            }

            return NotFound(_msg);
        }

    }

}