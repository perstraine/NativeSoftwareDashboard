using ConsoleUser.Data;
using ConsoleUser.Models;
using ConsoleUser.Models.Domain;
using ConsoleUser.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportLevelController : ControllerBase
    {
        
        //private readonly ICustomerRepository customerRepository;

        //public CustomerController(ICustomerRepository customerRepository)
        //{
        //    this.customerRepository = customerRepository;
        //}

        //[HttpGet]
        //public IActionResult GetAllCustomers()
        //{
        //    var customers = customerRepository.GetAll();

        //    //return DTO customers
        //    var customersDTO = new List<Models.DTO.Customer>();
        //    customers.ToList().ForEach(customer =>
        //    {
        //        var customerDTO = new Models.DTO.Customer()
        //        {
        //            CustomerCode = customer.CustomerCode,
        //            CustomerCodeZendesk = customer.CustomerCodeZendesk,
        //            SupportLevel = customer.SupportLevel,
        //        };

        //        customersDTO.Add(customerDTO);
        //    });
        //    return Ok(customersDTO);
        //}
    }
}
