using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using System.Net;
using LackLusterVideo.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Customers")]
    public class CustomerController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody] Customer NewCustInfo)
        {
            // Checks for nulls and such here...

            //if (credentials.username.ToString() == null || credentials.password == null)
            //{
            //    return BadRequest();
            //}

            CustomerUtils newCustUtil = new CustomerUtils();

            if (newCustUtil.MakeNewCustomer(NewCustInfo))
            {
                return Ok();
            }
            return BadRequest();
            
        }
    }
}