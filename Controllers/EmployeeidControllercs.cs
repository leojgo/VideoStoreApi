﻿using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Employee")]
    public class EmployeeidController : Controller
    {
        //===========================================
        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] EmployeeInfoToShare updatedInfo, int id)
        {
            if (updatedInfo != null)
            {
                var newEmployeeUtils = new EmployeeUtils();
                if (updatedInfo.FirstName != null)
                {
                    updatedInfo.EmployeeId = id;
                    if (newEmployeeUtils.EditEmployeeAccount(updatedInfo))
                    {
                        return NoContent();
                    }
                }
                return BadRequest("Invalid Input!!!");
            }
            else
            {
                var newEmployeeUtils = new EmployeeUtils();
                var empToClean = newEmployeeUtils.GetEmployeeById(id);
                if (empToClean != null)
                {
                    return Ok(empToClean);
                }

                return NotFound($"Couldnt find an Employee with the ID: {id}");
            }
        }
    }
}