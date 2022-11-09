using ConsoleUser.Data;
using ConsoleUser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ConsoleUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController2 : Controller
    {
        private readonly UsersAPIDbContext dbContext;
        private readonly IConfiguration _configuration;

        public CustomerController2(UsersAPIDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _configuration = configuration;
        }
    }
}
