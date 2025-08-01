using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSECSAPI.Data.Interface;
using SSECSAPI.Models;

namespace SSECSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public ICustomer _customer;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }
        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            return Ok(_customer.GetAllCustomer());
        }
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            return Ok(_customer.GetCustomerById(id));
        }
        [HttpPost]
        public IActionResult AddCategory(Customer customer)
        {
            return Ok(_customer.AddCustomer(customer));
        }
        [HttpPut]
        public IActionResult UpdateRole(Customer customer)
        {
            return Ok(_customer.UpdateCustomer(customer));
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            return Ok(_customer.DeleteCustomer(id));
        }
    }
}
