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
        public IEnumerable<EmployeeInfoToShare> GetAll()
        {
            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            IEnumerable<Employee> allEmployees = newEmployeeUtils.GetAllEmployees();
            List<EmployeeInfoToShare> CleanedEmployees = new List<EmployeeInfoToShare>();

            foreach (var employee in allEmployees)
            {
                CleanedEmployees.Add(RemovePersonalInfo(employee));
            }

            return CleanedEmployees;
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

        public static EmployeeInfoToShare RemovePersonalInfo(Employee toClean)
        {
            EmployeeInfoToShare cleanedInfo = new EmployeeInfoToShare();
            cleanedInfo.Active = toClean.Active;
            cleanedInfo.EmployeeId = toClean.EmployeeId;
            cleanedInfo.EmployeeTitle = toClean.EmployeeTitle;
            cleanedInfo.EmployeeType = toClean.EmployeeType;
            cleanedInfo.FirstName = toClean.FirstName;
            cleanedInfo.LastName = toClean.LastName;
            return cleanedInfo;
        }
    }

}