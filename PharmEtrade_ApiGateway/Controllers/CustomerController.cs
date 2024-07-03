using BAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Route("Login")]
        public async Task<IActionResult> CustomerLogin(string UserName, string Password)
        {
            var response = await _icustomerRepo.CustomerLogin(UserName,Password);
            if (response != null && response.LoginStatus == "Success")
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

        // Author: [Swathi]
        // Created Date: [01/07/2024]
        // Description: Method for adding product to cart
        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart(int userId, int imageId, int productId)
        {
            try
            {
                var result = await _icustomerRepo.AddToCart(userId, imageId, productId);
                if (result > 0)
                {
                    return Ok(new { Message = "Product added to cart successfully.", CartId = result });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to add product to cart." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return a server error response
                // Implement your logging mechanism here
                return StatusCode(500, new { Message = "An error occurred while adding the product to the cart.", Details = ex.Message });
            }
        }

        // Author: [shiva]
        // Created Date: [02/07/2024]
        // Description: Method for registration of User
        [HttpPost]
        [Route("UserRegistration")]
        public async Task<IActionResult> UserRegistration(UserViewModel userViewModel)
        {
            return Ok(await _icustomerRepo.UserRegistration(userViewModel));
        }
        // Author: [shiva]
        // Created Date: [02/07/2024]
        // Description: Method for Get the data Of Users From User Table 
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet]
        [Route("GetUserDetailsByUserId")]
        public async Task<IActionResult> GetUserDetailsByUserId(int userId)
        {
            return Ok(await _icustomerRepo.GetUserDetailsByUserId(userId));
        }

        // Author: [shiva]
        // Created Date: [03/07/2024]
        // Description: Method for Update Password
        [HttpPost]
        [Route("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(int id,string password)
        {
            return Ok(await _icustomerRepo.UpdatePassword(id,password));
        }

    }


}
