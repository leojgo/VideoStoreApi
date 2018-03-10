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
            CustomerUtils newCustUtil = new CustomerUtils();

            var item = newCustUtil.GetCustomerById(id);

            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        //DELETE, Non-Destructive
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            CustomerUtils newCustUtil = new CustomerUtils();
            if (newCustUtil.MakeCustomerInactive(id))
            {
                return NoContent();
            }

            return NotFound();
        }


        //Updates Customer
        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] Customer Customer)
        {
            //if (credentials.username.ToString() == null || credentials.password == null)
            //{
            //    return BadRequest();
            //}

            CustomerUtils newCustUtil = new CustomerUtils();
            bool result = newCustUtil.UpdateCustomer(Customer);

            if (result)
            {
                //SUCCESS
                _context.Customers.Add(Customer);
                _context.SaveChanges();
                return CreatedAtRoute("GetCustomer", new {id = Customer.CustomerId}, Customer);
            }

            return BadRequest();

        }
    }
}