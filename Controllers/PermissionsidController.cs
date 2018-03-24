using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Permissions")]
    public class PermissionsidController : Controller
    {
        

        private readonly PermissionContext _context;

        public PermissionsidController(PermissionContext context)
        {
            _context = context;
        }

        //===========================================

        [HttpGet("{id}", Name = "GetPermissions")]
        public IActionResult GetPermissions(int id)
        {
            try
            {
                PermissionUtils newPermissionUtils = new PermissionUtils();
                Permission toGet = new Permission();
                toGet = newPermissionUtils.GetPermissionById(id);

                if (toGet.EmpPermiss > 0)
                {
                    return new ObjectResult(toGet);
                }
                return NotFound($"There is no Info for Permission Level {id}");
            }
            catch (Exception e)
            {
                return NotFound($"There is no Info for Permission Level {id}");
            }
        }

    }
}