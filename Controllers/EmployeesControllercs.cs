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
        //===========================================
        [HttpPost]
        public IActionResult Create([FromBody] TempEmployee toCreate)
        {
            if (toCreate == null)
            {
                var newEmployeeUtils = new EmployeeUtils();

                return Json(newEmployeeUtils.GetAllEmployees());
            }
            else
            {
                var newEmployeeUtils = new EmployeeUtils();
                try
                {
                    if (toCreate.RawPw == null)
                    {
                        return BadRequest("Your Password is Missing!!!!");
                    }

                    var newEmpId = newEmployeeUtils.CreateNewUser(toCreate);
                    if (newEmpId > 0)
                    {
                        return Json(newEmpId);
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

}