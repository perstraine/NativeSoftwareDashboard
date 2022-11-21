using Microsoft.AspNetCore.Mvc;
using ConsoleUser.DTO;
using ConsoleUser.Repositories;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace ConsoleUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userRepository.AuthenticateAsync(loginRequest.UserEmail, loginRequest.Password);
            if (user !=null)
            {
                //generate token
                var token = await  tokenHandler.CreateTokenAsync(user);
                var returnObject = new { Token = token, userType = user.UserType };
                return Ok(returnObject);
            }
            return BadRequest("incorrect fields");
        }

        [HttpGet]
        [Route("login")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            return Ok("User Authorised");
        }
    }
}
