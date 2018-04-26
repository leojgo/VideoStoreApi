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
        //===========================================
        [HttpPost("{id}")]
        public IActionResult Create([FromBody] Login credentials, int id)
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
                    return Json(newEmployee);
                }
                return NotFound($"Couldn't Login Employee {credentials.Username}! ");
            }
            catch (Exception e)
            {
                return NotFound($"Couldn't Login Employee {credentials.Username}! " + e);
            }
        }
    }

    public class Login
    {
        public int Username { get; set; }
        public string Password { get; set; }
    }
}