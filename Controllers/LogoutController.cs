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
        //===========================================

        [HttpPost]
        public IActionResult Create([FromBody] Logout credentials)
        {
            var newEmpUtil = new EmployeeUtils();
            var toLogOut = newEmpUtil.ViewEmployeeAccount(credentials.Username);

            if (toLogOut == null)
            {
                return NotFound("Employee was Not Found!");
            }
            return new NoContentResult();
        }
    }

    public class Logout
    {
        public int Username { get; set; }
    }
}