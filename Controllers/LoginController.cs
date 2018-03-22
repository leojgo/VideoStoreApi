using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private string _msg;


        private readonly SessionContext _context;

        public LoginController(SessionContext context)
        {
            _context = context;
        }       

        //===========================================

        [HttpGet]
        public IEnumerable<EmployeeInfoToShare> GetAll()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id}", Name = "LoginEmployee")]
        public IActionResult GetById(int id)
        {
            var item = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (item == null)
            {
                _msg = "Employee Was Not Found!";
                return NotFound(_msg);
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Login credentials)
        {
            if (credentials.Username.ToString() == null || credentials.Password == null)
            {
                _msg = "Username or Password was not provided!";
                return BadRequest(_msg);
            }

            EmployeeUtils newEmpUtil = new EmployeeUtils();
            EmployeeInfoToShare newEmployee = newEmpUtil.LogIn(credentials.Username, credentials.Password, ref _msg);

            if (newEmployee != null)
            {
                _context.Employees.Add(newEmployee);
                _context.SaveChanges();
                return CreatedAtRoute("LoginEmployee", new {id = newEmployee.EmployeeId}, newEmployee );
            }

            return NotFound(_msg);
        }
    }

    public class Login
    {
        public int Username { get; set; }
        public string Password { get; set; }
    }
}