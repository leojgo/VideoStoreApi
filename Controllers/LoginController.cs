using System;
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
                return NotFound("Employee Was Not Found!");
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Login credentials)
        {
            if (credentials.Username.ToString() == null || credentials.Password == null)
            {
                return BadRequest("Username or Password was not provided!");
            }

            var newEmpUtil = new EmployeeUtils();

            try
            {
                var newEmployee = newEmpUtil.LogIn(credentials.Username, credentials.Password);

                if (newEmployee != null)
                {
                    if (!checkforSession(credentials.Username))
                    {
                        _context.Employees.Add(newEmployee);
                        _context.SaveChanges();
                        return CreatedAtRoute("LoginEmployee", new { id = newEmployee.EmployeeId }, newEmployee);
                    }
                    return Json(newEmployee);


                }
                return NotFound($"Couldn't Login Employee {credentials.Username}! ");
            }
            catch (Exception e)
            {
                return NotFound($"Couldn't Login Employee {credentials.Username}! " + e);
            }
        }

        public bool checkforSession(int id)
        {
            var item = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (item == null)
            {
                return false;
            }
            return true;
        }
    }

    public class Login
    {
        public int Username { get; set; }
        public string Password { get; set; }
    }
}