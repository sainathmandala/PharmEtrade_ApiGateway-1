using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Extensions;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IcustomerRepo _icustomerRepo;
        private readonly JwtAuthenticationExtensions _jwtTokenService;
        public CustomerController(IcustomerRepo icustomerRepo, JwtAuthenticationExtensions jwtTokenService)
        {
            _icustomerRepo = icustomerRepo;
            _jwtTokenService = jwtTokenService;
        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> CustomerLogin(string UserName, string Password)
        {
            var response = await _icustomerRepo.CustomerLogin(UserName,Password);
            if (response != null && response.LoginStatus== "Success")
            {
                return Ok(new
                {
                    Token = response.token,
                    //Username = response.Username,
                    //Role = response.Role
                });
            }

            return Unauthorized();
        }
    }
}
