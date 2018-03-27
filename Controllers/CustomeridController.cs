using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Customers")]
    public class CustomerIdController : Controller
    {
        private readonly CustomerContext _context;

        public CustomerIdController(CustomerContext context)
        {
            _context = context;
        }

        //===========================================

        [HttpGet]
        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult GetById(int id)
        {
            var newCustUtil = new CustomerUtils();

            var item = newCustUtil.GetCustomerById(id);

            if (item == null)
            {
                return NotFound("Customer Was Not Found");
            }

            return new ObjectResult(item);
        }

        //DELETE, Non-Destructive
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                var newCustUtil = new CustomerUtils();
                if (newCustUtil.MakeCustomerInactive(id))
                {
                    return NoContent();
                }

                return NotFound($"Couldnt find the Customer {id} you specified!");
            }
            catch (Exception e)
            {
                return NotFound($"Couldnt find the Customer {id} you specified!" + e);
            }
        }


        //Updates Customer
        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] Customer customer)
        {

            var newCustUtil = new CustomerUtils();
            try
            {
                var result = newCustUtil.UpdateCustomer(customer);

                if (result)
                {
                    //SUCCESS
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    return CreatedAtRoute("GetCustomer", new {id = customer.CustomerId}, customer);
                }
                return BadRequest($"Couldn't Update Customer!");
            }
            catch (Exception e)
            {
                return BadRequest($"Couldn't Update Customer!" + e);
            }
            

        }
    }
}