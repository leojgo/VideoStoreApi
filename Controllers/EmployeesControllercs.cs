using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
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

            return newEmployeeUtils.GetAllEmployees();
        }

        [HttpPost]
        public IActionResult Create([FromBody] TempEmployee toCreate)
        {
            EmployeeUtils newEmployeeUtils = new EmployeeUtils();
            try
            {
                if (toCreate.RawPw == null)
                {
                    return BadRequest("Your Password is Missing!!!!");
                }

            
                if(newEmployeeUtils.CreateNewUser(toCreate))
                {
                    return NoContent();
                }
                return BadRequest("Couldnt create the Employee!");
            }
            catch (Exception e)
            {
                return BadRequest("Couldnt create the Employee!" + e);
            }
            
        }
    }

}