using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/CustomerSearch")]
    public class CustomerSearchController : Controller
    {
        [HttpPost]
        public IActionResult Search([FromBody] CustomerSearchInfo searchInfo)
        {
            var searchResults = new List<Customer>();
            var newCustomerUtils = new CustomerUtils();

            searchResults = newCustomerUtils.SearchCustomers(searchInfo);

            if (searchResults.Count >= 1)
            {
                return new ObjectResult(searchResults);
            }

            return NotFound("No Results were found given the search terms entered!");
        }
    }

    public class CustomerSearchInfo
    {
        public string PhoneNumber { get; set; }
        public string SearchTerm { get; set; }
    }
}