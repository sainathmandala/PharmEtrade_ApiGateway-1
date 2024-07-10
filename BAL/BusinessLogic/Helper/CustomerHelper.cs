using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class CustomerHelper: IcustomerHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;
        private readonly SmtpSettings _smtpSettings;

        public CustomerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, SmtpSettings smtpSettings)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _smtpSettings = smtpSettings;
        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        public async Task<DataTable> CustomerLogin(string username, string password)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("SP_CustomerLogin", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", username);
                cmd.Parameters.AddWithValue("@Password", password);
                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "CustomerLogin_SP :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        public async Task<int> AddToCart(int userId, int imageId, int productId)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("InsertAddtoCartProduct", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Userid", userId);
                cmd.Parameters.AddWithValue("@Imageid", imageId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                return await Task.Run(() => _isqlDataHelper.ExcuteNonQueryasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "AddToCart_SP :  errormessage:" + ex.Message.ToString()));
                throw ex;
            }
        }

        public Task<int> dummy(int userId, int imageId, int productId)
        {
            throw new NotImplementedException();
        }

        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for registration of User 
        public async Task<string> SaveCustomerData(UserViewModel userViewModel)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("SP_InsertUser", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userViewModel.Username);
                cmd.Parameters.AddWithValue("@Email", userViewModel.Email);
                cmd.Parameters.AddWithValue("@Password", userViewModel.Password);
                cmd.Parameters.AddWithValue("@PhoneNumber", userViewModel.PhoneNumber);
                cmd.Parameters.AddWithValue("@RoleId", userViewModel.RoleId);

                await sqlcon.OpenAsync();
                await _isqlDataHelper.ExcuteNonQueryasync(cmd);
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveUser :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }


        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for Get the data of  User based on UserId
        public async Task<DataTable> GetUserDetailsById(int userId)
        {

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_GetUserById", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", userId);


                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetusersdataById_sp :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        // Author: [Shiva]
        // Created Date: [03/07/2024]
        // Description: Method for update password
        public async Task<string> UpdatePassword(int id,  string newPassword)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_UpdatePassword", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
               

                await sqlcon.OpenAsync();
                string result = await cmd.ExecuteScalarAsync() as string;
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "UpdatePassword: ErrorMessage - " + ex.Message.ToString()));
                throw ex;
            }
        }


        // Author: [Shiva]
        // Created Date: [04/07/2024]
        // Description: Method for Get the data of  User based on Email
        public async Task<DataTable> GetUserDetailsByEmail(string email)
        {

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_GetUserByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);


                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetusersdataByEmail_sp :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        // Author: [Shiva]
        // Created Date: [08/07/2024]
        // Description: Method for update password by email(reset Password)
        public async Task<string> UpdatePasswordByEmail(string email, string newPassword)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_UpdatePasswordByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);


                await sqlcon.OpenAsync();
                string result = await cmd.ExecuteScalarAsync() as string;
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "UpdatePasswordBymail: ErrorMessage - " + ex.Message.ToString()));
                throw ex;
            }
        }

        // Author: [Shiva]
        // Created Date: [10/07/2024]
        // Description: Method for Send Otp
        public async Task<string> SendOTPEmail(string email)
        {
            var otp = GenerateOTP();
            var otpExpiration = DateTime.Now.AddMinutes(5);
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("sp_GenerateAndStoreOTP", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@OTP", otp);
                cmd.Parameters.AddWithValue("@OTPExpiration", otpExpiration);

                await sqlcon.OpenAsync();
                string result = await cmd.ExecuteScalarAsync() as string;
                await SendEmailAsync(email, otp);
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SendOTPEmail: ErrorMessage - " + ex.Message.ToString()));
                throw ex;
            }
        }

        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public async Task SendEmailAsync(string toEmail, string Otp)
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
                var subject = "Otp Request";
                var message = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>OTP Code</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        .header, .footer {{
            background-color: #007BFF;
            color: #ffffff;
            text-align: center;
            padding: 10px 0;
            border-radius: 8px 8px 0 0;
        }}
        .footer {{
            border-radius: 0 0 8px 8px;
            font-size: 12px;
            margin-top: 20px;
            border-top: none;
        }}
        .logo img {{
            max-width: 100px;
            height: auto;
        }}
        .content {{
            text-align: left;
            padding: 20px 0;
        }}
        .otp-code {{
            font-size: 24px;
            font-weight: bold;
            color: #333333;
            margin-bottom: 10px;
        }}
        .message {{
            margin-bottom: 20px;
        }}
        .footer p {{
            margin: 0;
        }}
        @media only screen and (max-width: 600px) {{
            .container {{
                padding: 10px;
            }}
            .header, .footer {{
                padding: 15px 5px;
            }}
            .logo img {{
                max-width: 80px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <div class=""logo"">
                <img src=""https://your-company-logo-url.png"" alt=""Company Logo"">
            </div>
        </div>
        <div class=""content"">
            <p>Dear User,</p>
            <p>Your OTP code for verification is:</p>
            <p class=""otp-code"">{Otp}</p>
            <p class=""message"">Please use this code within the next 5 minutes to complete your login process.</p>
            <p>Thank you,</p>
            <p>PharmETrade</p>
        </div>
        <div class=""footer"">
            <p>This email was sent automatically. Please do not reply.</p>
        </div>
    </div>
</body>
</html>

";
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
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SendEmailAsync_helper: ErrorMessage - " + ex.Message));
                throw;
            }
        }

        // Author: [Shiva]
        // Created Date: [10/07/2024]
        // Description: Method for Otp login
        public async Task<DataTable> OtpLogin(string email, string otp)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("sp_ValidateOTP", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@OTP", otp);
                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "OtpLogin :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

    }
}
