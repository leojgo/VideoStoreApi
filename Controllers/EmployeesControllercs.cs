using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private string _msg;

        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }       

        //===========================================

        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            //return _context.Employees.ToList();
            IEnumerable<Employee> allEmployees;

            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            allEmployees = newEmployeeUtils.GetAllEmployees();

            return allEmployees;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TempEmployee toCreate)
        {
            if (toCreate.RawPw == null)
            {
                _msg = "Your Password is Missing!!!!";
                return BadRequest(_msg);
            }

            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            if(newEmployeeUtils.CreateNewUser(toCreate, ref _msg))
            {
                return NoContent();
            }

            return BadRequest(_msg);
        }
    }

}