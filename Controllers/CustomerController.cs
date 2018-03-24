using System;
using Microsoft.AspNetCore.Mvc;
using LackLusterVideo.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Customers")]
    public class CustomerController : Controller
    {
        private string _msg;
        [HttpPost]
        public IActionResult Create([FromBody] Customer newCustInfo)
        {
            CustomerUtils newCustUtil = new CustomerUtils();

            newCustInfo.AccountBalance = 0;
            newCustInfo.Active = true;

            try
            {
                ReturnedKey newCustomerKey = new ReturnedKey();
                newCustomerKey.Key = newCustUtil.MakeNewCustomer(newCustInfo, ref _msg);
                if (newCustomerKey.Key > -1)
                {
                    return Ok(newCustomerKey);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            
            return BadRequest(_msg);
        }
    }

    public class ReturnedKey
    {
        public int Key { get; set; }
    }
}