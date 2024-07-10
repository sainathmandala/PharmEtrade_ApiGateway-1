using BAL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PharmEtrade_ApiGateway.Extensions;
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

        // Author: [shiva]
        // Created Date: [04/07/2024]
        // Description: Method for Get the data Of Users From User Table by email
     
        [HttpGet]
        [Route("GetUserDetailsByEmail")]
        public async Task<IActionResult> GetUserDetailsByEmail(string email)
        {
            return Ok(await _icustomerRepo.GetUserDetailsByEmail(email));
        }



        // Author: [shiva]
        // Created Date: [04/07/2024]
        // Description: Method for Forgot Password

        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {

            var user = await _icustomerRepo.ForgotPassword(email);
            if (user == null)
            {
                // Do not reveal that the user does not exist
                return Ok(new { message = "If this email is registered, a password reset link will be sent." });
            }

            // Generate password reset token
            var token = _jwtTokenService.GenerateToken(user.Email, user.Role);

            // Generate reset link
            var resetLink = Url.Action("GetUserDetailsByEmail", "Customer", new { token, email = user.Email }, Request.Scheme);

            // Compose email
            var subject = "Password Reset Request";
            var message = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: 'Arial', sans-serif;
            background-color: #f7f7f7;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #004d99;
            color: #ffffff;
            text-align: center;
            padding: 20px 0;
        }}
        .header img {{
            max-width: 150px;
        }}
        .body {{
            padding: 20px;
            text-align: left;
            color: #333333;
        }}
        .body h1 {{
            color: #004d99;
            font-size: 24px;
            margin-bottom: 20px;
        }}
        .body p {{
            color: #555555;
            line-height: 1.6;
            font-size: 16px;
        }}
        .reset-button {{
            display: inline-block;
            padding: 12px 25px;
            margin: 20px 0;
            color: #eceef1;
            background-color: #b6d4f1;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
        }}
        .footer {{
            text-align: center;
            padding: 15px 0;
            color: #888888;
            font-size: 14px;
            border-top: 1px solid #e0e0e0;
            background-color: #f0f0f0;
        }}
        .footer p {{
            margin: 0;
        }}
        .footer a {{
            color: #004d99;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <img src='https://localhost:7189/Images/logo_04.png' alt='Company Logo' />
        </div>
        <div class='body'>
            <h1>Password Reset Request</h1>
            <p>Hello,</p>
            <p>We received a request to reset your password. Please click the button below to reset it:</p>
            <a href='{resetLink}' class='reset-button'>Reset Password</a>
            <p>If you did not request this password reset, please ignore this email. Your account remains secure.</p>
            <p>For any assistance, please contact our support team at <a href='mailto:support@example.com'>support@example.com</a>.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 PharmEtrade Company. All rights reserved.</p>
            <p><a href='https://example.com/privacy'>Privacy Policy</a> | <a href='https://example.com/contact'>Contact Us</a></p>
        </div>
    </div>
</body>
</html>
";
            // Send email
            await _icustomerRepo.SendEmailAsync(user.Email, subject, message);

            return Ok(new { message = "If this email is registered, a password reset link will be sent." });
        }

        // Author: [shiva]
        // Created Date: [08/07/2024]
        // Description: Method for ResetPassword
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _icustomerRepo.GetUserDetailsByEmail(resetPasswordDto.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok("Password reset link has been sent to your email.");
            }

            // Validate the JWT token
            if (!ValidateToken(resetPasswordDto.Token,resetPasswordDto.Email))
            {
                return BadRequest("Invalid token.");
            }

            // Update the password
            var result = await _icustomerRepo.UpdatePasswordByEmail(resetPasswordDto.Email, resetPasswordDto.NewPassword);

            if (result!=null)
            {
                return Ok("Password has been reset successfully.");
            }

           

            return BadRequest();
        }

        // Author: [shiva]
        // Created Date: [08/07/2024]
        // Description: Method for validating token 
        private bool ValidateToken(string token, string expectedUsername)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Adjust as necessary
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

                // Ensure the username in the token matches the expected username
                return username == expectedUsername;
            }
            catch
            {
                // Token validation failed
                return false;
            }
        }

        // Author: [shiva]
        // Created Date: [10/07/2024]
        // Description: Method for Send Otp
        [HttpPost]
        [Route("SendOtp")]
        public async Task<IActionResult> SendOtp(string Email)
        {
            return Ok(await _icustomerRepo.SendOTPEmail(Email));
        }
        // Author: [Shiva]
        // Created Date: [10/07/2024]
        // Description: Method for  login with Otp
        [HttpPost]
        [Route("LoginWithOtp")]
        public async Task<IActionResult> LoginWithOtp(string email, string otp)
        {
            var response = await _icustomerRepo.OtpLogin(email, otp);
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
    }


}
