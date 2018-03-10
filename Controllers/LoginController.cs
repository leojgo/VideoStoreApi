using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private readonly EmployeeContext _context;

        public LoginController(EmployeeContext context)
        {
            _context = context;
        }       

        //===========================================

        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public IActionResult GetById(int id)
        {
            var item = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Login credentials)
        {
            if (credentials.username.ToString() == null || credentials.password == null)
            {
                return BadRequest();
            }

            EmployeeUtils newEmpUtil = new EmployeeUtils();
            Employee toLogIn = newEmpUtil.LogIn(credentials.username, credentials.password);

            if (toLogIn != null)
            {
                _context.Employees.Add(toLogIn);
                _context.SaveChanges();
                return CreatedAtRoute("GetEmployee", new {id = toLogIn.EmployeeId}, toLogIn );
            }
            return NotFound();
            
        }
    }

    public class Login
    {
        public int username { get; set; }
        public string password { get; set; }
    }
}