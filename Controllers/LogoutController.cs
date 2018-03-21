using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Logout")]
    public class LogoutController : Controller
    {
        private string _msg;

        private readonly SessionContext _context;

        public LogoutController(SessionContext context)
        {
            _context = context;
        }       

        //===========================================

        [HttpGet]
        public IEnumerable<EmployeeInfoToShare> GetAll()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id}", Name = "LogOut Employee")]
        public IActionResult GetById(int id)
        {
            var item = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (item == null)
            {
                _msg = "Employee was Not Found!";
                return NotFound(_msg);
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Logout credentials)
        {
            EmployeeUtils newEmpUtil = new EmployeeUtils();
            Employee toLogOut = newEmpUtil.ViewEmployeeAccount(credentials.Username);

            if (toLogOut == null)
            {
                _msg = "Employee was Not Found!";
                return NotFound(_msg);
            }

            EmployeeInfoToShare toLogOutClean = RemovePersonalInfo(toLogOut);

            _context.Employees.Remove(toLogOutClean);
            _context.SaveChanges();
            return new NoContentResult();
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

    public class Logout
    {
        public int Username { get; set; }
    }
}