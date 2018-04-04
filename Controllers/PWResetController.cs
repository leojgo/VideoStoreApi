using System;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/resetPassword")]
    public class PwResetController : Controller
    {
        [HttpPost("{id}")]
        public IActionResult ResetPw([FromBody] PWResetInfo newCustInfo, int id)
        {
            if (newCustInfo.ManagerInfo.Username.ToString() == null || newCustInfo.ManagerInfo.Password == null)
            {
                return BadRequest("Username or Password was not provided!");
            }

            var newEmpUtil = new EmployeeUtils();

            try
            {
                var newEmployee = newEmpUtil.LogIn(newCustInfo.ManagerInfo.Username, newCustInfo.ManagerInfo.Password);

                if (newEmployee != null)
                {
                    var newEmployeeUtils = new EmployeeUtils();
                    var empToClean = newEmployeeUtils.GetEmployeeById(id);
                    if (empToClean != null)
                    {
                        Login updatedInfo = new Login();
                        updatedInfo.Username = id;
                        updatedInfo.Password = newCustInfo.NewPw;

                        if (newEmployeeUtils.EditEmployeePW(updatedInfo))
                        {
                            return Ok();
                        }
                        return StatusCode(500, "Error, Couldnt update the User Password! ");
                    }
                }
                return NotFound($"Couldn't Login Employee {newCustInfo.ManagerInfo.Username}! ");
            }
            catch (Exception e)
            {
                return NotFound($"You Broke something, Great Job!! " + e);
            }
        }
    }

    public class PWResetInfo
    {
        public Login ManagerInfo { get; set; }
        public string NewPw { get; set; }

    }
}