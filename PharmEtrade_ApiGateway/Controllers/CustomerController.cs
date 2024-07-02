using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IcustomerRepo _icustomerRepo;
        public CustomerController(IcustomerRepo icustomerRepo)
        {
            _icustomerRepo = icustomerRepo;
        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> CustomerLogin(string UserName, string Password)
        {
            //return Ok(await _icustomerRepo.(UserName, Password));
            return null;
        }

    }
}
