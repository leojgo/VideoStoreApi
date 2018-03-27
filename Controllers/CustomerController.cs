using System;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Customers")]
    public class CustomerController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody] Customer newCustInfo)
        {
            var newCustUtil = new CustomerUtils();

            newCustInfo.AccountBalance = 0;
            newCustInfo.Active = true;

            try
            {
                var newCustomerKey = new ReturnedKey();
                newCustomerKey.Key = newCustUtil.MakeNewCustomer(newCustInfo);
                if (newCustomerKey.Key > -1)
                {
                    return Ok(newCustomerKey);
                }
                return BadRequest("Error, Customer Couldnt be created!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Error, Customer Couldnt be created! " + e);
            }
        }
    }

    public class ReturnedKey
    {
        public int Key { get; set; }
    }
}