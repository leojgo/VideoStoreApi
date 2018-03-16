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

            if (newCustUtil.MakeNewCustomer(newCustInfo, ref _msg))
            {
                return Ok();
            }
            return BadRequest(_msg);
        }
    }
}