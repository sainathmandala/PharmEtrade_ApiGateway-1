using BAL.BusinessLogic.Helper;
using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;
using PharmEtrade_ApiGateway.Extensions;
using DAL.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class CustomerRepository:IcustomerRepo
    {
        private readonly IcustomerHelper _icustomerHelper;
        private readonly JwtAuthenticationExtensions _jwtTokenService;
      
        private readonly IConfiguration _configuration;
        private readonly SmtpSettings _smtpSettings;

        public CustomerRepository(IcustomerHelper icustomerHelper, JwtAuthenticationExtensions jwtTokenService, IConfiguration configuration, IOptions<SmtpSettings> smtpSettings)
        {
            _icustomerHelper = icustomerHelper;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _smtpSettings = smtpSettings.Value;
        }

        public Task<int> AddToCart(int userId, int imageId, int productId)
        {
            throw new NotImplementedException();
        }


        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for customer login 
        public async Task<loginViewModel> CustomerLogin(string username, string password)
        {
            loginViewModel response = new loginViewModel();

            try
            {
                var dtResult = await _icustomerHelper.CustomerLogin(username, password);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow row = dtResult.Rows[0];
                    response.LoginStatus = row["LoginStatus"].ToString();
                    response.UserId = Convert.ToInt32(row["UserId"]);
                    response.Username = row["Username"].ToString();
                    response.UserEmail = row["UserEmail"].ToString();
                    response.Role = row["Role"].ToString();

                    if (response.LoginStatus == "Success")
                    {
                        response.token = _jwtTokenService.GenerateToken(response.Username, response.Role);
                        response.statusCode = 200;
                        //response.message = LoginSuccessMsg;
                    }
                    else
                    {
                        response.statusCode = 400;
                        response.message = "Invalid credentials or user role not found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = $"An error occurred: {ex.Message}";
            }

            return response;

        }
        // Author: [shiva]
        // Created Date: [02/07/2024]
        // Description: Method for registration of User
        public async Task<Response> UserRegistration(UserViewModel userViewModel)
        {
            Response response = new Response();
            try
            {
                string status = await _icustomerHelper.SaveCustomerData(userViewModel);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.UserCreationSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }


        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for Get the data of User Based On UserId
        public async Task<UserDetailsResponse> GetUserDetailsByUserId(int userId)
        {
            UserDetailsResponse response = new UserDetailsResponse();
            try
            {
                DataTable dtresult = await _icustomerHelper.GetUserDetailsById(userId);
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    response.statusCode = 200;
                    response.message = Constant.GetUserBYUserIdSuccessMsg;
                    response.userlist = ConvertDataTabletoStudentList(dtresult);
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = ex.Message;
                response.userlist = new List<UserViewModel>();

            }
            return response;
        }

        private List<UserViewModel> ConvertDataTabletoStudentList(DataTable dt)
        {
            List<UserViewModel> userlst = new List<UserViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    UserViewModel user = new UserViewModel();
                    user.UserId = Convert.ToInt32(row["user_id"]);
                    user.Username = row["username"].ToString();
                    user.Email = row["email"].ToString();
                    user.Password = row["password"].ToString();
                    user.PhoneNumber =row["phone_number"].ToString();
                  

                    userlst.Add(user);

                }
                return userlst;
            }
            else
                return userlst;
        }

        // Author: [shiva]
        // Created Date: [03/07/2024]
        // Description: Method for Update  Password
        public async Task<Response> UpdatePassword(int id,string newPassword)
        {
            Response response = new Response();
            try
            {
                string status = await _icustomerHelper.UpdatePassword(id, newPassword);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.UpdatePasswordSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }

        // Author: [Shiva]
        // Created Date: [04/07/2024]
        // Description: Method for Get the data of User Based On email
        public async Task<UserEmailResponse> GetUserDetailsByEmail(string email)
        {
            UserEmailResponse response = new UserEmailResponse();
            try
            {
                DataTable dtresult = await _icustomerHelper.GetUserDetailsByEmail(email);
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    response.statusCode = 200;
                    response.message = Constant.GetUserBYUserIdSuccessMsg;
                    response.userlist = ConvertDataTabletoUserEmailtList(dtresult);
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = ex.Message;
                response.userlist = new List<UserEmailViewModel>();

            }
            return response;
        }
        private List<UserEmailViewModel> ConvertDataTabletoUserEmailtList(DataTable dt)
        {
            List<UserEmailViewModel> userlst = new List<UserEmailViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    UserEmailViewModel user = new UserEmailViewModel();
                    user.UserId = Convert.ToInt32(row["user_id"]);
                    user.Username = row["username"].ToString();
                    user.Email = row["email"].ToString();
                    user.PhoneNumber = row["phone_number"].ToString();
                    user.Role = row["role"].ToString();


                    userlst.Add(user);

                }
                return userlst;
            }
            else
                return userlst;
        }

        // Author: [Shiva]
        // Created Date: [o4/07/2024]
        // Description: Method for Forgot Password
        public async Task<UserEmailViewModel> ForgotPassword(string email)
        {
            UserEmailViewModel response = new UserEmailViewModel();

            try
            {
                var dtResult = await _icustomerHelper.GetUserDetailsByEmail(email);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow row = dtResult.Rows[0];
                  
                    response.UserId = Convert.ToInt32(row["user_id"]);
                    response.Username = row["username"].ToString();
                    response.Email = row["email"].ToString();
                    response.PhoneNumber = row["phone_number"].ToString();
                    response.Role = row["role"].ToString();

                  
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }

        // Author: [Shiva]
        // Created Date: [o4/07/2024]
        // Description: Method for Send Mail
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                if (_smtpSettings == null)
                {
                    throw new InvalidOperationException("SMTP settings are not configured.");
                }

                if (string.IsNullOrEmpty(_smtpSettings.Host) || _smtpSettings.Port == 0 ||
                    string.IsNullOrEmpty(_smtpSettings.Username) || string.IsNullOrEmpty(_smtpSettings.Password))
                {
                    throw new InvalidOperationException("One or more SMTP settings are not configured properly.");
                }

                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.EnableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.Username),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Author: [shiva]
        // Created Date: [08/07/2024]
        // Description: Method for Update  Password By mail (Reset Password)
        public async Task<Response> UpdatePasswordByEmail(string mail, string newPassword)
        {
            Response response = new Response();
            try
            {
                string status = await _icustomerHelper.UpdatePasswordByEmail(mail, newPassword);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.UpdatePasswordSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }

        // Author: [shiva]
        // Created Date: [10/07/2024]
        // Description: Method for Send Otp TO mail
        public async Task<Response> SendOTPEmail(string email)
        {
            Response response = new Response();
            try
            {
                string status = await _icustomerHelper.SendOTPEmail(email);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.SendOtpSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }
        // Author: [Shiva]
        // Created Date: [10/07/2024]
        // Description: Method for  login  with Otp
        public async Task<loginViewModel> OtpLogin(string email, string otp)
        {
            loginViewModel response = new loginViewModel();

            try
            {
                var dtResult = await _icustomerHelper.OtpLogin(email, otp);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow row = dtResult.Rows[0];
                    response.LoginStatus = row["LoginStatus"].ToString();
                    response.UserId = Convert.ToInt32(row["user_id"]);
                    response.Username = row["username"].ToString();
                    response.UserEmail = row["email"].ToString();
                    response.Role = row["role"].ToString();

                    if (response.LoginStatus == "Success")
                    {
                        response.token = _jwtTokenService.GenerateToken(response.Username, response.Role);
                        response.statusCode = 200;
                        //response.message = LoginSuccessMsg;
                    }
                    else
                    {
                        response.statusCode = 400;
                        response.message = Constant.OtpExperiedMsg;
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = $"An error occurred: {ex.Message}";
            }

            return response;

        }
    }
}
