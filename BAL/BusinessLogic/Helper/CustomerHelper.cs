using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using DAL;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;
using BAL.ResponseModels;
using Microsoft.AspNetCore.Http;
using BAL.Models;
using BAL.RequestModels;

namespace BAL.BusinessLogic.Helper
{
    public class CustomerHelper : IcustomerHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;
        private readonly SmtpSettings _smtpSettings;
        private readonly S3Helper _s3Helper;
        private readonly IEmailHelper _emailHelper;

        public CustomerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, SmtpSettings smtpSettings , IEmailHelper emailHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _smtpSettings = smtpSettings;
            _s3Helper = new S3Helper(configuration);
            _emailHelper = emailHelper;

        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        public async Task<DataTable> CustomerLogin(string username, string password)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("SP_Login", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Email", username);
                cmd.Parameters.AddWithValue("p_Password", password);
                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "Login_SP :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        public async Task<int> AddToCart(int userId, int imageId, int productId)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("InsertAddtoCartProduct", sqlcon);
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
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("SP_InsertUser", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_firstname", userViewModel.firstname);
                cmd.Parameters.AddWithValue("p_lastname", userViewModel.lastname);
                cmd.Parameters.AddWithValue("p_Email", userViewModel.Email);
                cmd.Parameters.AddWithValue("p_Password", userViewModel.Password);
                cmd.Parameters.AddWithValue("p_PhoneNumber", userViewModel.PhoneNumber);
                cmd.Parameters.AddWithValue("p_usertypeid", userViewModel.UsertypeId);
                cmd.Parameters.AddWithValue("p_accounttype", userViewModel.Accounttype);
                cmd.Parameters.AddWithValue("p_upnmember", userViewModel.UpnMember);
                cmd.Parameters.AddWithValue("p_otp", null);
                cmd.Parameters.AddWithValue("p_otp_expiration", null);

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

            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("Sp_GetUserById", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_user_id", userId);


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
        public async Task<string> UpdatePassword(int id, string newPassword)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("Sp_UpdatePassword", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_UserId", id);
                cmd.Parameters.AddWithValue("p_NewPassword", newPassword);


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

            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("Sp_GetUserByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Email", email);


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
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("Sp_UpdatePasswordByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Email", email);
                cmd.Parameters.AddWithValue("p_NewPassword", newPassword);


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
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("sp_GenerateAndStoreOTP", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Email", email);
                cmd.Parameters.AddWithValue("p_OTP", otp);
                cmd.Parameters.AddWithValue("p_OTPExpiration", otpExpiration);

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
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("sp_ValidateOTP", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Email", email);
                cmd.Parameters.AddWithValue("p_OTP", otp);
                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "OtpLogin :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }


        // Author: [Shiva]
        // Created Date: [04/08/2024]
        // Description: Method for Save the data of Business Info Of User
        public async Task<string> SaveBusinessInfoData(BusinessInfoViewModel businessInfo)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            string deaLicenseS3Path = null;
            string pharmacyLicenseS3Path = null;
            string folderName = "User_BusinessInfo";
            try
            {
                // Upload files to S3
                if (businessInfo.DEAlicenseCopy != null)
                {
                    deaLicenseS3Path = await _s3Helper.UploadFileAsync(businessInfo.DEAlicenseCopy, folderName);
                }

                if (businessInfo.PharmacyLicenseCopy != null)
                {
                    pharmacyLicenseS3Path = await _s3Helper.UploadFileAsync(businessInfo.PharmacyLicenseCopy, folderName);
                }

                await sqlcon.OpenAsync();
                using (var transaction = await sqlcon.BeginTransactionAsync())
                {
                    cmd = new MySqlCommand("SP_InsertBusinessInfo", sqlcon, transaction);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_user_id", businessInfo.UserId);
                    cmd.Parameters.AddWithValue("p_shop_name", businessInfo.ShopName);
                    cmd.Parameters.AddWithValue("p_dba", businessInfo.DBA);
                    cmd.Parameters.AddWithValue("p_legal_business_name", businessInfo.LegalBusinessName);
                    cmd.Parameters.AddWithValue("p_address", businessInfo.Address);
                    cmd.Parameters.AddWithValue("p_city", businessInfo.City);
                    cmd.Parameters.AddWithValue("p_state", businessInfo.State);
                    cmd.Parameters.AddWithValue("p_zip", businessInfo.Zip);
                    cmd.Parameters.AddWithValue("p_business_phone", businessInfo.BusinessPhone);
                    cmd.Parameters.AddWithValue("p_business_fax", businessInfo.BusinessFax);
                    cmd.Parameters.AddWithValue("p_business_email", businessInfo.BusinessEmail);
                    cmd.Parameters.AddWithValue("p_federal_tax_id", businessInfo.FederalTaxId);
                    cmd.Parameters.AddWithValue("p_dea", businessInfo.DEA);
                    cmd.Parameters.AddWithValue("p_pharmacy_licence", businessInfo.PharmacyLicence);
                    cmd.Parameters.AddWithValue("p_dea_expiration_date", businessInfo.DEAExpirationDate);
                    cmd.Parameters.AddWithValue("p_pharmacy_license_expiration_date", businessInfo.PharmacyLicenseExpirationDate);
                    cmd.Parameters.AddWithValue("p_dea_license_copy", deaLicenseS3Path); // Use S3 path
                    cmd.Parameters.AddWithValue("p_pharmacy_license_copy", pharmacyLicenseS3Path); // Use S3 path
                    cmd.Parameters.AddWithValue("p_npi", businessInfo.NPI);
                    cmd.Parameters.AddWithValue("p_ncpdp", businessInfo.NCPDP);

                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && result.ToString() == "Business info inserted successfully.")
                    {
                        await transaction.CommitAsync();
                        return "Success";
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Stored procedure execution failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveBusinessInfoData: errormessage:" + ex.Message.ToString()));

                //Delete files from S3 if stored procedure fails
                if (!string.IsNullOrEmpty(deaLicenseS3Path))
                {
                    await _s3Helper.DeleteFileAsync($"User_BusinessInfo/{businessInfo.DEAlicenseCopy.FileName}");
                }

                if (!string.IsNullOrEmpty(pharmacyLicenseS3Path))
                {
                    await _s3Helper.DeleteFileAsync($"User_BusinessInfo/{businessInfo.PharmacyLicenseCopy.FileName}");
                }

                throw ex;
            }
        }

        public async Task<string> AddUpdateCustomer(ViewModels.Customer customer)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    await sqlcon.OpenAsync();
                    cmd = new MySqlCommand("sp_AddCustomer", sqlcon);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("p_Email", customer.Email);
                    cmd.Parameters.AddWithValue("p_Password", customer.Password);
                    cmd.Parameters.AddWithValue("p_Mobile", customer.Mobile);
                    cmd.Parameters.AddWithValue("p_CustomerTypeId", customer.CustomerTypeId);
                    cmd.Parameters.AddWithValue("p_AccountTypeId", customer.AccountTypeId);
                    cmd.Parameters.AddWithValue("p_IsUPNMember", customer.IsUPNMember);
                    cmd.Parameters.AddWithValue("p_LoginOTP", null);
                    cmd.Parameters.AddWithValue("p_OTPExpiryDate", null);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader["Status"].ToString() ?? "";
<<<<<<< HEAD
                        string _mailBody = EmailTemplates.CUSTOMER_TEMPLATE;
                        _mailBody = _mailBody.Replace("{{CustomerId}}", customer.CustomerId);
                       // _mailBody = _mailBody.Replace("{{RegistrationDetailsHTML}}", GetCustomerDetailsHTml(customer));
                        await _emailHelper.SendEmail(customer.Email, "", " Registration is completed  Successfully ", _mailBody);
=======
                        //string _mailBody = EmailTemplates.CUSTOMER_TEMPLATE;
                        //_mailBody = _mailBody.Replace("{{CustomerId}}", customer.CustomerId);
                        //_mailBody = _mailBody.Replace("{{RegistrationDetailsHTML}}", GetCustomerDetailsHTml(customer));
                        //await _emailHelper.SendEmail(customer.Email, "", " Registration is completed  Successfully ", _mailBody);
>>>>>>> 36849daee67800059ebcbe2538db8a0f3cf6cd72
                    }
                    return "";


                }
                catch (Exception ex)
                {
                    Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "AddUpdate Customer :  errormessage:" + ex.Message.ToString()));
                    return "ERROR : " + ex.Message;
                }
            }
        }
<<<<<<< HEAD
        //private string GetCustomerDetailsHTml(CustomerResponse customer)
        //{
        //    string _GetCustomerDetailsHTml = "";
        //     int sNumber = 1;
        //    foreach (var details in customer.CustomerDetails)
        //    {
        //        _GetCustomerDetailsHTml += "<tr>";
        //        _GetCustomerDetailsHTml += string.Format("<td> {0} </td>", sNumber);
        //        _GetCustomerDetailsHTml += string.Format("<td> {0} </td>", details.FirstName);
        //        _GetCustomerDetailsHTml += string.Format("<td> {0} </td>", details.Email);
        //        _GetCustomerDetailsHTml += string.Format("<td> {0} </td>", details.Password);
        //        _GetCustomerDetailsHTml += string.Format("<td> {0} </td>", details.Mobile);
        //        _GetCustomerDetailsHTml +=string.Format("<td> {0}</td>",  details.CustomerTypeId);
        //        _GetCustomerDetailsHTml += string.Format("<td> {0}</td>", details.AccountTypeId);
        //        _GetCustomerDetailsHTml += "</tr>";
        //        sNumber++;
        //    }
        //    _GetCustomerDetailsHTml += "<tr style='font-weight:bold'><td colspan='4'></td>";
        //    _GetCustomerDetailsHTml += "</tr>";
=======
        private string GetCustomerDetailsHTml(CustomerResponse customer)
        {
            string _GetCustomerDetailsHTml = "";
             int sNumber = 1;
           
                _GetCustomerDetailsHTml += "<tr>";
                //_GetCustomerDetailsHTml += string.Format("<td> {0} </td>", sNumber);
                //_GetCustomerDetailsHTml += string.Format("<td> {0} </td>", customer.FirstName);
                //_GetCustomerDetailsHTml += string.Format("<td> {0} </td>", customer.Email);
                //_GetCustomerDetailsHTml += string.Format("<td> {0} </td>", customer.Password);
                //_GetCustomerDetailsHTml += string.Format("<td> {0} </td>", customer.Mobile);
                //_GetCustomerDetailsHTml +=string.Format("<td> {0}</td>", customer.CustomerTypeId);
                //_GetCustomerDetailsHTml += string.Format("<td> {0}</td>", customer.AccountTypeId);
                _GetCustomerDetailsHTml += "</tr>";
                sNumber++;
           
            _GetCustomerDetailsHTml += "<tr style='font-weight:bold'><td colspan='4'></td>";
            _GetCustomerDetailsHTml += "</tr>";
>>>>>>> 36849daee67800059ebcbe2538db8a0f3cf6cd72

        //    return _GetCustomerDetailsHTml;

        //}

        public async Task<UploadResponse> UploadImage(IFormFile image)
        {
            UploadResponse response = new UploadResponse();
            string folderName = "User_BusinessInfo";
            try
            {
                // Upload files to S3
                if (image != null)
                {
                    response.ImageUrl = await _s3Helper.UploadFileAsync(image, folderName);
                    response.Status = 200;
                    response.Message = "Image Uploaded Successfully.";
                }
            }
            catch (Exception ex)
            {
                Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveBusinessInfoData: errormessage:" + ex.Message.ToString()));

                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<string> AddUpdateBusinessInfo(ViewModels.CustomerBusinessInfo businessInfo)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                await sqlcon.OpenAsync();
                using (var transaction = await sqlcon.BeginTransactionAsync())
                {
                    cmd = new MySqlCommand("sp_AddUpdateBusinessInfo", sqlcon, transaction);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_CustomerId", businessInfo.CustomerId);
                    cmd.Parameters.AddWithValue("p_ShopName", businessInfo.ShopName);
                    cmd.Parameters.AddWithValue("p_DBA", businessInfo.DBA);
                    cmd.Parameters.AddWithValue("p_LegalBusinessName", businessInfo.LegalBusinessName);
                    cmd.Parameters.AddWithValue("p_Address", businessInfo.Address);
                    cmd.Parameters.AddWithValue("p_City", businessInfo.City);
                    cmd.Parameters.AddWithValue("p_State", businessInfo.State);
                    cmd.Parameters.AddWithValue("p_Zip", businessInfo.Zip);
                    cmd.Parameters.AddWithValue("p_BusinessPhone", businessInfo.BusinessPhone);
                    cmd.Parameters.AddWithValue("p_BusinessFax", businessInfo.BusinessFax);
                    cmd.Parameters.AddWithValue("p_BusinessEmail", businessInfo.BusinessEmail);
                    cmd.Parameters.AddWithValue("p_FederalTaxId", businessInfo.FederalTaxId);
                    cmd.Parameters.AddWithValue("p_DEA", businessInfo.DEA);
                    cmd.Parameters.AddWithValue("p_PharmacyLicence", businessInfo.PharmacyLicence);
                    cmd.Parameters.AddWithValue("p_DEAExpirationDate", businessInfo.DEAExpirationDate);
                    cmd.Parameters.AddWithValue("p_PharmacyLicenseExpirationDate", businessInfo.PharmacyLicenseExpirationDate);
                    cmd.Parameters.AddWithValue("p_DEALicenseCopy", businessInfo.DEALicenseCopy); // Use S3 path
                    cmd.Parameters.AddWithValue("p_PharmacyLicenseCopy", businessInfo.PharmacyLicenseCopy); // Use S3 path
                    cmd.Parameters.AddWithValue("p_NPI", businessInfo.NPI);
                    cmd.Parameters.AddWithValue("p_NCPDP", businessInfo.NCPDP);
                    cmd.Parameters.AddWithValue("p_CompanyWebsite", businessInfo.CompanyWebsite);

                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && result.ToString() == "Business Info Updated successfully.")
                    {
                        await transaction.CommitAsync();
                        return "SUCCESS";
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return "ERROR";
                    }
                }
            }
            catch (Exception ex)
            {
                Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveBusinessInfoData: errormessage:" + ex.Message.ToString()));

                return "ERROR : " + ex.Message;
            }
        }

        public async Task<Response<CustomerResponse>> GetCustomerByCustomerId(string customerId)
        {
            var response = new Response<CustomerResponse>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_GetCustomerByCustomerId", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CustomerId", customerId);

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblCustomer = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblCustomer.Rows.Count == 0)
                        {
                            throw new Exception("Customer not found.");
                        }

                        var customer = new ViewModels.Customer();
                        var businessInfoResponse = new ViewModels.CustomerBusinessInfo(); // Use correct type

                        foreach (DataRow row in tblCustomer.Rows)
                        {
                            customer.CustomerId = row["CustomerId"] != DBNull.Value ? row["CustomerId"].ToString() : null;
                            customer.FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : null;
                            customer.LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : null;
                            customer.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null;
                            customer.Mobile = row["Mobile"] != DBNull.Value ? row["Mobile"].ToString() : null;
                            customer.Password = row["Password"] != DBNull.Value ? row["Password"].ToString() : null;
                            customer.CustomerTypeId = row["CustomerTypeId"] != DBNull.Value ? Convert.ToInt32(row["CustomerTypeId"]) : default;
                            customer.AccountTypeId = row["AccountTypeId"] != DBNull.Value ? Convert.ToInt32(row["AccountTypeId"]) : default;
                            customer.IsUPNMember = row["IsUPNMember"] != DBNull.Value ? Convert.ToInt32(row["IsUPNMember"]) : default;  // Use Convert.ToBoolean
                            customer.LoginOTP = row["LoginOTP"] != DBNull.Value ? row["LoginOTP"].ToString() : null;
                            customer.OTPExpiryDate = row["OTPExpiryDate"] != DBNull.Value ? Convert.ToDateTime(row["OTPExpiryDate"]) : (DateTime?)null;

                            // Map data to BusinessInfoResponse
                            businessInfoResponse.CustomerBusinessInfoId = row["CustomerBusinessInfoId"] != DBNull.Value ? Convert.ToInt32(row["CustomerBusinessInfoId"]) : default;

                            businessInfoResponse.ShopName = row["ShopName"] != DBNull.Value ? row["ShopName"].ToString() : null;
                            businessInfoResponse.DBA = row["DBA"] != DBNull.Value ? row["DBA"].ToString() : null;
                            businessInfoResponse.LegalBusinessName = row["LegalBusinessName"] != DBNull.Value ? row["LegalBusinessName"].ToString() : null;
                            businessInfoResponse.Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : null;
                            businessInfoResponse.City = row["City"] != DBNull.Value ? row["City"].ToString() : null;
                            businessInfoResponse.State = row["State"] != DBNull.Value ? row["State"].ToString() : null;
                            businessInfoResponse.Zip = row["Zip"] != DBNull.Value ? row["Zip"].ToString() : null;
                            businessInfoResponse.BusinessPhone = row["BusinessPhone"] != DBNull.Value ? row["BusinessPhone"].ToString() : null;
                            businessInfoResponse.BusinessFax = row["BusinessFax"] != DBNull.Value ? row["BusinessFax"].ToString() : null;
                            businessInfoResponse.BusinessEmail = row["BusinessEmail"] != DBNull.Value ? row["BusinessEmail"].ToString() : null;
                            businessInfoResponse.FederalTaxId = row["FederalTaxId"] != DBNull.Value ? row["FederalTaxId"].ToString() : null;
                            businessInfoResponse.DEA = row["DEA"] != DBNull.Value ? row["DEA"].ToString() : null;
                            businessInfoResponse.PharmacyLicence = row["PharmacyLicence"] != DBNull.Value ? row["PharmacyLicence"].ToString() : null;
                            businessInfoResponse.DEAExpirationDate = row["DEAExpirationDate"] != DBNull.Value ? Convert.ToDateTime(row["DEAExpirationDate"]) : (DateTime?)null;
                            businessInfoResponse.PharmacyLicenseExpirationDate = row["PharmacyLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(row["PharmacyLicenseExpirationDate"]) : (DateTime?)null;

                            // Assuming DEALicenseCopy and PharmacyLicenseCopy are paths/URLs, not file uploads
                            businessInfoResponse.DEALicenseCopy = row["DEALicenseCopy"] != DBNull.Value ? row["DEALicenseCopy"].ToString() : null;
                            businessInfoResponse.PharmacyLicenseCopy = row["PharmacyLicenseCopy"] != DBNull.Value ? row["PharmacyLicenseCopy"].ToString() : null;

                            businessInfoResponse.NPI = row["NPI"] != DBNull.Value ? row["NPI"].ToString() : null;
                            businessInfoResponse.NCPDP = row["NCPDP"] != DBNull.Value ? row["NCPDP"].ToString() : null;
                        }

                        var customerData = new CustomerResponse()
                        {
                            CustomerDetails = customer,
                            BusinessInfo = businessInfoResponse
                        };

                        response.StatusCode = 200;
                        response.Message = "Successfully fetched data.";
                        response.Result = new List<CustomerResponse>() { customerData };
                    }
                    catch (MySqlException ex)
                    {
                        // Handle MySQL exceptions
                        throw new Exception("An error occurred while retrieving the customer.", ex);
                    }
                    catch (Exception ex)
                    {
                        // Handle general exceptions
                        throw new Exception("An unexpected error occurred.", ex);
                    }
                    return response;
                }
            }
        }

        public async Task<Response<ViewModels.Customer>> GetCustomers(string? customerId, string? email, string? mobile)
        {
            var response = new Response<ViewModels.Customer>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_GetCustomers", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@p_Email", email);
                    cmd.Parameters.AddWithValue("@p_Mobile", mobile);

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblCustomer = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblCustomer.Rows.Count == 0)
                        {
                            throw new Exception("Customer not found.");
                        }

                        var lstCustomers = new List<ViewModels.Customer>();
                        foreach (DataRow row in tblCustomer.Rows)
                        {
                            var customer = new ViewModels.Customer();
                            customer.CustomerId = row["CustomerId"] != DBNull.Value ? row["CustomerId"].ToString() : null;
                            customer.FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : null;
                            customer.LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : null;
                            customer.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null;
                            customer.Mobile = row["Mobile"] != DBNull.Value ? row["Mobile"].ToString() : null;
                            customer.Password = row["Password"] != DBNull.Value ? row["Password"].ToString() : null;
                            customer.CustomerTypeId = row["CustomerTypeId"] != DBNull.Value ? Convert.ToInt32(row["CustomerTypeId"]) : default;
                            customer.AccountTypeId = row["AccountTypeId"] != DBNull.Value ? Convert.ToInt32(row["AccountTypeId"]) : default;
                            customer.IsUPNMember = row["IsUPNMember"] != DBNull.Value ? Convert.ToInt32(row["IsUPNMember"]) : default;  // Use Convert.ToBoolean
                            customer.LoginOTP = row["LoginOTP"] != DBNull.Value ? row["LoginOTP"].ToString() : null;
                            customer.OTPExpiryDate = row["OTPExpiryDate"] != DBNull.Value ? Convert.ToDateTime(row["OTPExpiryDate"]) : (DateTime?)null;
                            lstCustomers.Add(customer);
                        }

                        response.StatusCode = 200;
                        response.Message = "Successfully fetched data.";
                        response.Result = lstCustomers;
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                        response.Result = null;
                    }
                    return response;
                }
            }
        }

        public async Task<Response<Address>> GetByCustomerId(string customerId)
        {
            Response<Address> response = new Response<Address>();
            try
            {
                MySqlCommand cmdAddress = new MySqlCommand(StoredProcedures.CUSTOMER_GET_ALL_ADDRESSES);
                cmdAddress.CommandType = CommandType.StoredProcedure;

                cmdAddress.Parameters.AddWithValue("@p_CustomerId", customerId);

                DataTable tblAddress = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdAddress));

                response.StatusCode = 200;
                response.Message = "Address(es) fetched Successfully.";
                response.Result = MapDataTableToAddressList(tblAddress);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Address>> GetAddressById(string addressId)
        {
            Response<Address> response = new Response<Address>();
            try
            {
                MySqlCommand cmdAddress = new MySqlCommand(StoredProcedures.CUSTOMER_GET_ADDRESS);
                cmdAddress.CommandType = CommandType.StoredProcedure;

                cmdAddress.Parameters.AddWithValue("@p_AddressId", addressId);

                DataTable tblAddress = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdAddress));

                response.StatusCode = 200;
                response.Message = "Address(es) fetched Successfully.";
                response.Result = MapDataTableToAddressList(tblAddress);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        private List<Address> MapDataTableToAddressList(DataTable tblAddress)
        {
            List<Address> lstAddress = new List<Address>();
            foreach (DataRow aItem in tblAddress.Rows)
            {
                Address address = new Address();
                address.AddressId = aItem["AddressId"].ToString() ?? "";
                address.CustomerId = aItem["CustomerId"].ToString() ?? "";
                address.FirstName = aItem["FirstName"].ToString() ?? "";
                address.MiddleName = aItem["MiddleName"].ToString() ?? "";
                address.LastName = aItem["LastName"].ToString() ?? "";
                address.Address1 = aItem["Address1"].ToString() ?? "";
                address.Address2 = aItem["Address2"].ToString() ?? "";
                address.PhoneNumber = aItem["PhoneNumber"].ToString() ?? "";
                address.Pincode = aItem["Pincode"].ToString() ?? "";
                address.City = aItem["City"].ToString() ?? "";
                address.State = aItem["State"].ToString() ?? "";
                address.Country = aItem["Country"].ToString() ?? "";
                address.Landmark = aItem["Landmark"].ToString() ?? "";
                address.DeliveryInstructions = aItem["DeliveryInstructions"].ToString() ?? "";
                address.AddressTypeId = Convert.ToInt32(Convert.IsDBNull(aItem["AddressTypeId"]) ? 0 : aItem["AddressTypeId"]);
                address.IsDefault = Convert.ToInt32(Convert.IsDBNull(aItem["IsDefault"]) ? 0 : aItem["IsDefault"]) == 0 ? false : true;
                lstAddress.Add(address);
            }
            return lstAddress;
        }

        public async Task<Response<Address>> AddUpdateAddress(Address customerAddress)
        {
            Response<Address> response = new Response<Address>();
            try
            {
                MySqlCommand cmdAddress = new MySqlCommand(StoredProcedures.CUSTOMER_ADD_UPDATE_ADDRESS);
                cmdAddress.CommandType = CommandType.StoredProcedure;

                cmdAddress.Parameters.AddWithValue("@p_AddressId", customerAddress.AddressId);
                cmdAddress.Parameters.AddWithValue("@p_CustomerId", customerAddress.CustomerId);
                cmdAddress.Parameters.AddWithValue("@p_FirstName", customerAddress.FirstName);
                cmdAddress.Parameters.AddWithValue("@p_MiddleName", customerAddress.MiddleName);
                cmdAddress.Parameters.AddWithValue("@p_LastName", customerAddress.LastName);
                cmdAddress.Parameters.AddWithValue("@p_Address1", customerAddress.Address1);
                cmdAddress.Parameters.AddWithValue("@p_Address2", customerAddress.Address2);
                cmdAddress.Parameters.AddWithValue("@p_PhoneNumber", customerAddress.PhoneNumber);
                cmdAddress.Parameters.AddWithValue("@p_City", customerAddress.City);
                cmdAddress.Parameters.AddWithValue("@p_State", customerAddress.State);
                cmdAddress.Parameters.AddWithValue("@p_Country", customerAddress.Country);
                cmdAddress.Parameters.AddWithValue("@p_Pincode", customerAddress.Pincode);
                cmdAddress.Parameters.AddWithValue("@p_IsDefault", customerAddress.IsDefault ? 1 : 0);
                cmdAddress.Parameters.AddWithValue("@p_DeliveryInstructions", customerAddress.DeliveryInstructions);
                cmdAddress.Parameters.AddWithValue("@p_Landmark", customerAddress.Landmark);
                cmdAddress.Parameters.AddWithValue("@p_AddressTypeId", customerAddress.AddressTypeId);

                DataTable tblAddress = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdAddress));

                response.StatusCode = 200;
                response.Message = "Address Added/Updated Successfully.";
                response.Result = MapDataTableToAddressList(tblAddress);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Address>> DeleteAddress(string addressId)
        {
            Response<Address> response = new Response<Address>();
            try
            {
                MySqlCommand cmdAddress = new MySqlCommand(StoredProcedures.CUSTOMER_ADD_UPDATE_ADDRESS);
                cmdAddress.CommandType = CommandType.StoredProcedure;

                cmdAddress.Parameters.AddWithValue("@p_AddressId", addressId);

                DataTable tblAddress = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdAddress));

                response.StatusCode = 200;
                response.Message = "Address Deleted Successfully.";
                response.Result = MapDataTableToAddressList(tblAddress);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
