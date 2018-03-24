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
                return NotFound("Employee was Not Found!");
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Logout credentials)
        {
            EmployeeUtils newEmpUtil = new EmployeeUtils();
            EmployeeInfoToShare toLogOut = newEmpUtil.ViewEmployeeAccount(credentials.Username);

            if (toLogOut == null)
            {
                return NotFound("Employee was Not Found!");
            }

            _context.Employees.Remove(toLogOut);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }

    public class Logout
    {
        public int Username { get; set; }
    }
}