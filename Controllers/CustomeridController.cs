using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using LackLusterVideo.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Customers")]
    public class CustomerIdController : Controller
    {
        private string _msg;

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
                _msg = "Customer Was Not Found";
                return NotFound();
            }

            return new ObjectResult(item);
        }

        //DELETE, Non-Destructive
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            CustomerUtils newCustUtil = new CustomerUtils();
            if (newCustUtil.MakeCustomerInactive(id, ref _msg))
            {
                return NoContent();
            }

            return NotFound(_msg);
        }


        //Updates Customer
        [HttpPost("{id}")]
        public IActionResult UpdateInfo([FromBody] Customer customer)
        {
            //if (credentials.username.ToString() == null || credentials.password == null)
            //{
            //    return BadRequest();
            //}

            CustomerUtils newCustUtil = new CustomerUtils();
            bool result = newCustUtil.UpdateCustomer(customer, ref _msg);

            if (result)
            {
                //SUCCESS
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return CreatedAtRoute("GetCustomer", new {id = customer.CustomerId}, customer);
            }
            return BadRequest(_msg);

        }
    }
}