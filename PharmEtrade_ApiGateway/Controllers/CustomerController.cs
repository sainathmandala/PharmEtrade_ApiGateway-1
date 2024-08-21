using BAL.ResponseModels;
using BAL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PharmEtrade_ApiGateway.Extensions;
using PharmEtrade_ApiGateway.Repository.Helper;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IcustomerRepo _icustomerRepo;
        private readonly JwtAuthenticationExtensions _jwtTokenService;
        private readonly IConfiguration _configuration;

        public CustomerController(IcustomerRepo icustomerRepo, JwtAuthenticationExtensions jwtTokenService, IConfiguration configuration)
        {
            _icustomerRepo = icustomerRepo;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> CustomerLogin(string UserName, string Password)
        {
            var response = await _icustomerRepo.CustomerLogin(UserName, Password);
            if (response != null && response.LoginStatus == "Success")
            {
                return Ok(response);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Register new customer or update existing customer - Only for Registration Process
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterCustomer(Customer customer)
        {
            RegistrationResponse response = await _icustomerRepo.RegisterCustomer(customer);
            return Ok(response);
        }

        /// <summary>
        /// Uploads an Image to AWS S3 bucket and return the url
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            UploadResponse response = await _icustomerRepo.UploadImage(image);
            return Ok(response);
        }

        [HttpPost]
        [Route("BusinessInfo")]
        public async Task<IActionResult> BusinessInfo(CustomerBusinessInfo businessInfo)
        {
            BusinessInfoResponse response = await _icustomerRepo.AddUpdateBusinessInfo(businessInfo);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetByCustomerId")]
        public async Task<IActionResult> GetCustomersByCustomerId(string customerId)
        {  
            try
            {               
                var response = await _icustomerRepo.GetCustomerByCustomerId(customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {               
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving customer details.");
            }
        }

    }
}
