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
using System.Reflection;
using BAL.RequestModels.Customer;

namespace BAL.BusinessLogic.Helper
{
    public class CustomerHelper : ICustomerHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;
        private readonly SmtpSettings _smtpSettings;
        private readonly S3Helper _s3Helper;
        private readonly IEmailHelper _emailHelper;
        private readonly IJwtHelper _jwtHelper;
        private readonly string _uiURL;

        public CustomerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, SmtpSettings smtpSettings , IJwtHelper jwtHelper, IEmailHelper emailHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
            _uiURL = configuration.GetSection("PharmEtradeSettings")["UIUrl"] ?? "";
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _smtpSettings = smtpSettings;
            _s3Helper = new S3Helper(configuration);
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
        }
                
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

        public async Task<LoginResponse> AdminLogin(string adminId, string password)
        {
            var response = new LoginResponse();
            using (MySqlCommand command = new MySqlCommand(StoredProcedures.CUSTOMER_ADMIN_LOGIN))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_AdminId", adminId);
                command.Parameters.AddWithValue("p_Password", password);

                try
                {
                    DataTable tblResult = await _isqlDataHelper.ExecuteDataTableAsync(command);
                    if (tblResult != null && tblResult.Rows.Count > 0)
                    {
                        response.StatusCode = "SUCCESS".Equals(tblResult.Rows[0]["LoginStatus"].ToString() ?? "") ? 200 : 500;
                        response.Message = tblResult.Rows[0]["Message"].ToString() ?? "";
                        response.UserId = tblResult.Rows[0]["UserId"].ToString() ?? "";
                        response.Email = tblResult.Rows[0]["Email"].ToString() ?? "";
                        response.Firstname = tblResult.Rows[0]["Firstname"].ToString() ?? "";
                        response.Lastname = tblResult.Rows[0]["Lastname"].ToString() ?? "";
                        response.UserType = tblResult.Rows[0]["UserType"].ToString() ?? "";
                        response.UserTypeId = Convert.ToInt32(tblResult.Rows[0]["UserTypeId"] != DBNull.Value ? tblResult.Rows[0]["UserTypeId"] : 0);
                        response.Token = _jwtHelper.GenerateToken(response.Email, response.UserType);
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "No records found for the Buyer";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = "ERROR : " + ex.Message;
                }
            }

            return response;
        }

        public async Task<string> AddCustomer(CustomerAddRequest customer)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    await sqlcon.OpenAsync();
                    cmd = new MySqlCommand("sp_AddCustomer", sqlcon);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@p_LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@p_Email", customer.Email);
                    cmd.Parameters.AddWithValue("@p_Password", customer.Password);
                    cmd.Parameters.AddWithValue("@p_Mobile", customer.Mobile);
                    cmd.Parameters.AddWithValue("@p_CustomerTypeId", customer.CustomerTypeId);
                    cmd.Parameters.AddWithValue("@p_AccountTypeId", customer.AccountTypeId);
                    cmd.Parameters.AddWithValue("@p_IsUPNMember", customer.IsUPNMember);
                    cmd.Parameters.AddWithValue("@p_LoginOTP", null);
                    cmd.Parameters.AddWithValue("@p_OTPExpiryDate", null);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        //var registraionMailBody = EmailTemplates.CUSTOMER_TEMPLATE;
                        //registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", customer.Email);
                        //registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", customer.Email);
                        //registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", customer.FirstName + ' ' + customer.LastName);
                        //await _emailHelper.SendEmail(customer.Email, "", "Your registration is successfull.", registraionMailBody);
                        return reader["Status"].ToString() ?? "";                        
                    }
                    return "";


                }
                catch (Exception ex)
                {
                    //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "AddUpdate Customer :  errormessage:" + ex.Message.ToString()));
                    return "ERROR : " + ex.Message;
                }
            }
        }

        public async Task<string> EditCustomer(CustomerEditRequest customer)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    await sqlcon.OpenAsync();
                    cmd = new MySqlCommand(StoredProcedures.CUSTOMER_EDIT_PROFILE, sqlcon);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CustomerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@p_FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@p_LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@p_Email", customer.Email);
                    cmd.Parameters.AddWithValue("@p_Password", customer.Password);
                    cmd.Parameters.AddWithValue("@p_Mobile", customer.Mobile);
                    cmd.Parameters.AddWithValue("@p_CustomerTypeId", customer.CustomerTypeId);
                    cmd.Parameters.AddWithValue("@p_AccountTypeId", customer.AccountTypeId);
                    cmd.Parameters.AddWithValue("@p_IsUPNMember", customer.IsUPNMember);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var registraionMailBody = EmailTemplates.CUSTOMER_EDIT_TEMPLATE;
                        registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", customer.Email);
                        registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", customer.Email);
                        registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", customer.FirstName + ' ' + customer.LastName);
                        await _emailHelper.SendEmail(customer.Email, "", "Your profile Has been updated successfully", registraionMailBody);
                        return reader["Status"].ToString() ?? "";
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "AddUpdate Customer :  errormessage:" + ex.Message.ToString()));
                    return "ERROR : " + ex.Message;
                }
            }
        }

        public async Task<string> DeleteCustomer(string customerId)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    await sqlcon.OpenAsync();
                    cmd = new MySqlCommand(StoredProcedures.CUSTOMER_DELETE, sqlcon);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CustomerId", customerId);                    

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        //var registraionMailBody = EmailTemplates.CUSTOMER_EDIT_TEMPLATE;
                        //registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", customer.Email);
                        //registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", customer.Email);
                        //registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", customer.FirstName + ' ' + customer.LastName);
                        //await _emailHelper.SendEmail(customer.Email, "", "Your profile Has been updated successfully", registraionMailBody);
                        return reader["Message"].ToString() ?? "";
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    return "ERROR : " + ex.Message;
                }
            }
        }

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
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveBusinessInfoData: errormessage:" + ex.Message.ToString()));

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

                    // var result = await cmd.ExecuteScalarAsync();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        await transaction.CommitAsync();
                        reader.Read();
                        if (businessInfo.SendRegistrationMail)
                        {
                            var registraionMailBody = EmailTemplates.CUSTOMER_TEMPLATE;
                            registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", reader["Email"].ToString() ?? "");
                            registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", reader["Email"].ToString() ?? "");
                            registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", reader["FirstName"].ToString() ?? "" + ' ' + reader["LastName"].ToString() ?? "");
                            await _emailHelper.SendEmail(reader["Email"].ToString() ?? "", "", "Your registration is successfull.", registraionMailBody);
                        }
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
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveBusinessInfoData: errormessage:" + ex.Message.ToString()));

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
                            customer.CreatedDate= row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : (DateTime?)null;
                            customer.ModifiedDate = row["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedDate"]) : (DateTime?)null;
                            customer.ActivationDate = row["ActivationDate"] != DBNull.Value ? Convert.ToDateTime(row["ActivationDate"]) : (DateTime?)null;
                            customer.ShopName = row["ShopName"] != DBNull.Value ? row["ShopName"].ToString() : null;
                            customer.IsActive = Convert.ToInt32(Convert.IsDBNull(row["IsActive"]) ? 0 : row["IsActive"]);

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
                            customer.CreatedDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : (DateTime?)null;
                            customer.ModifiedDate = row["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedDate"]) : (DateTime?)null;
                            customer.ActivationDate = row["ActivationDate"] != DBNull.Value ? Convert.ToDateTime(row["ActivationDate"]) : (DateTime?)null;
                            customer.ShopName = row["ShopName"] != DBNull.Value ? row["ShopName"].ToString() : null;
                            customer.IsActive = Convert.ToInt32(Convert.IsDBNull(row["IsActive"]) ? 0 : row["IsActive"]);
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

        public async Task<Response<string>> SendChangePasswordLink(string customerId)
        {
            var response = new Response<string>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_SetChangePasswordStatus", sqlcon))
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

                        string customerEmail = tblCustomer.Rows[0]["Email"].ToString() ?? "";

                        string chgPwdLink = _uiURL + "changepassword?token=" + _jwtHelper.GenerateToken(customerEmail, "PharmEtradeUser");

                        response.StatusCode = 200;
                        response.Message = "Successfully fetched data.";
                        response.Result = new List<string>() { chgPwdLink };

                        string changePasswordMailBody = EmailTemplates.CUSTOMER_CHANGEPASSWORD_TEMPLATE;
                        changePasswordMailBody = changePasswordMailBody.Replace("{{CHANGE_PASSWORD_URL}}", chgPwdLink);
                        await _emailHelper.SendEmail(customerEmail, "", "Change Password", changePasswordMailBody);
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

        public async Task<Response<ViewModels.Customer>> ChangePassword(string customerId, string newPassword)
        {
            var response = new Response<ViewModels.Customer>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ChagePassword", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@p_NewPassword", newPassword);

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
                            customer.CreatedDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : (DateTime?)null;
                            customer.ModifiedDate = row["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedDate"]) : (DateTime?)null;
                            customer.ActivationDate = row["ActivationDate"] != DBNull.Value ? Convert.ToDateTime(row["ActivationDate"]) : (DateTime?)null;
                            customer.ShopName = row["ShopName"] != DBNull.Value ? row["ShopName"].ToString() : null;
                            customer.IsActive = Convert.ToInt32(Convert.IsDBNull(row["IsActive"]) ? 0 : row["IsActive"]);
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

        public async Task<Response<Customer>> GetByFilterCriteria(CustomerFilterCriteria filterCriteria) {
            var response = new Response<ViewModels.Customer>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_GetCustomersByCriteria", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@p_CustomerName", filterCriteria.CustomerName);
                    cmd.Parameters.AddWithValue("@p_CustomerTypeId", filterCriteria.CustomerTypeId);

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
                            customer.CreatedDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : (DateTime?)null;
                            customer.ModifiedDate = row["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedDate"]) : (DateTime?)null;
                            customer.ActivationDate = row["ActivationDate"] != DBNull.Value ? Convert.ToDateTime(row["ActivationDate"]) : (DateTime?)null;
                            customer.ShopName = row["ShopName"] != DBNull.Value ? row["ShopName"].ToString() : null;
                            customer.IsActive = Convert.ToInt32(Convert.IsDBNull(row["IsActive"]) ? 0 : row["IsActive"]);
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

        public async Task<Response<CustomerAddress>> GetByCustomerId(string customerId)
        {
            Response<CustomerAddress> response = new Response<CustomerAddress>();
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

        public async Task<Response<CustomerAddress>> GetAddressById(string addressId)
        {
            Response<CustomerAddress> response = new Response<CustomerAddress>();
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

        private List<CustomerAddress> MapDataTableToAddressList(DataTable tblAddress)
        {
            List<CustomerAddress> lstAddress = new List<CustomerAddress>();
            foreach (DataRow aItem in tblAddress.Rows)
            {
                CustomerAddress address = new CustomerAddress();
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
            
                address.IsDefault = Convert.ToInt32(Convert.IsDBNull(aItem["IsDefault"]) ? 0 : aItem["IsDefault"]) == 0 ? false : true;
                lstAddress.Add(address);
            }
            return lstAddress;
        }

        public async Task<Response<CustomerAddress>> AddUpdateAddress(CustomerAddress customerAddress)
        {
            Response<CustomerAddress> response = new Response<CustomerAddress>();
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

        public async Task<Response<CustomerAddress>> DeleteAddress(string addressId)
        {
            Response<CustomerAddress> response = new Response<CustomerAddress>();
            try
            {
                MySqlCommand cmdAddress = new MySqlCommand(StoredProcedures.CUSTOMER_DELETE_ADDRESS);
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

        public async Task<Response<string>> Activate(string customerId, string? comments)
        {
            Response<string> response = new Response<string>();
            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.CUSTOMER_ACTIVATE_DEACTIVATE);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_CustomerId", customerId);
                command.Parameters.AddWithValue("@p_Comments", comments);
                command.Parameters.AddWithValue("@p_IsActive", 1);

                MySqlDataReader reader = await _isqlDataHelper.ExecuteReaderAsync(command);

                if(reader.HasRows)
                {
                    reader.Read();
                    response.StatusCode = 200;
                    response.Message = "SUCCESS : Command Execution";
                    response.Result = new List<string>() { reader["Message"].ToString() ?? "" };

                    //Send Email to customer
                    var registraionMailBody = EmailTemplates.CUSTOMER_ACTIVATE_DEACTIVATE_TEMPLATE;
                    registraionMailBody = registraionMailBody.Replace("{{CUST_STATUS}}", reader["Action"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", reader["Email"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", reader["Email"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", reader["CustomerFullName"].ToString() ?? "");
                    await _emailHelper.SendEmail(reader["Email"].ToString() ?? "", "", "Your account has been " + reader["Action"].ToString() ?? "", registraionMailBody);
                }
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<string>> Deactivate(string customerId, string? comments)
        {
            Response<string> response = new Response<string>();
            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.CUSTOMER_ACTIVATE_DEACTIVATE);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_CustomerId", customerId);
                command.Parameters.AddWithValue("@p_Comments", comments);
                command.Parameters.AddWithValue("@p_IsActive", 0);

                MySqlDataReader reader = await _isqlDataHelper.ExecuteReaderAsync(command);

                if (reader.HasRows)
                {
                    reader.Read();
                    response.StatusCode = 200;
                    response.Message = "SUCCESS : Command Execution";
                    response.Result = new List<string>() { reader["Message"].ToString() ?? "" };

                    //Send Email to customer
                    var registraionMailBody = EmailTemplates.CUSTOMER_ACTIVATE_DEACTIVATE_TEMPLATE;
                    registraionMailBody = registraionMailBody.Replace("{{CUST_STATUS}}", reader["Action"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CustomerId}}", reader["Email"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CUST_EMAIL}}", reader["Email"].ToString() ?? "");
                    registraionMailBody = registraionMailBody.Replace("{{CUST_FULL_NAME}}", reader["CustomerFullName"].ToString() ?? "");
                    await _emailHelper.SendEmail(reader["Email"].ToString() ?? "", "", "Your account has been " + reader["Action"].ToString() ?? "", registraionMailBody);
                }
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<BeneficiaryDetails>> AddUpdateBeneficiaryDetail(BeneficiaryDetails beneficiaryDetails)
        {
            Response<BeneficiaryDetails> response = new Response<BeneficiaryDetails>();
            try
            {
                MySqlCommand cmdBeneficiary = new MySqlCommand(StoredProcedures.CUSTOMER_ADD_UPDATE_BENEFICIARYDETAILS);
                cmdBeneficiary.CommandType = CommandType.StoredProcedure;
                cmdBeneficiary.Parameters.AddWithValue("@p_BeneficiaryId", beneficiaryDetails.Id);
                cmdBeneficiary.Parameters.AddWithValue("@p_CustomerId", beneficiaryDetails.CustomerId);
                cmdBeneficiary.Parameters.AddWithValue("@p_BankName", beneficiaryDetails.BankName);
                cmdBeneficiary.Parameters.AddWithValue("@p_BankAddress", beneficiaryDetails.BankAddress);
                cmdBeneficiary.Parameters.AddWithValue("@p_RoutingNumber", beneficiaryDetails.RoutingNumber);
                cmdBeneficiary.Parameters.AddWithValue("@p_AccountNumber", beneficiaryDetails.AccountNumber);
                cmdBeneficiary.Parameters.AddWithValue("@p_AccountType", beneficiaryDetails.AccountType);
                cmdBeneficiary.Parameters.AddWithValue("@p_CheckPayableTo", beneficiaryDetails.CheckPayableTo);
                cmdBeneficiary.Parameters.AddWithValue("@p_FirstName", beneficiaryDetails.FirstName);
                cmdBeneficiary.Parameters.AddWithValue("@p_LastName", beneficiaryDetails.LastName);
                cmdBeneficiary.Parameters.AddWithValue("@p_AddressLine1", beneficiaryDetails.AddressLine1);
                cmdBeneficiary.Parameters.AddWithValue("@p_AddressLine2", beneficiaryDetails.AddressLine2);
                cmdBeneficiary.Parameters.AddWithValue("@p_City", beneficiaryDetails.City);
                cmdBeneficiary.Parameters.AddWithValue("@p_State", beneficiaryDetails.State);
                cmdBeneficiary.Parameters.AddWithValue("@p_Zip", beneficiaryDetails.Zip);
       
                DataTable tblBeneficiary = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdBeneficiary));

                response.StatusCode = 200;
                response.Message = "Beneficiary Added/Updated Successfully.";
                response.Result = MapDataTableToBeneficiaryList(tblBeneficiary);
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
        private List<BeneficiaryDetails> MapDataTableToBeneficiaryList(DataTable tblBeneficiary)
        {
            List<BeneficiaryDetails> lstBeneficiary = new List<BeneficiaryDetails>();
            foreach (DataRow aItem in tblBeneficiary.Rows)
            {
                BeneficiaryDetails beneficiary = new BeneficiaryDetails();
                beneficiary.Id = aItem["Id"].ToString() ?? "";
                beneficiary.CustomerId = aItem["CustomerId"].ToString() ?? "";
                beneficiary.BankName = aItem["BankName"].ToString() ?? "";
                beneficiary.BankAddress = aItem["BankAddress"].ToString() ?? "";
                beneficiary.RoutingNumber = aItem["RoutingNumber"].ToString() ?? "";
                beneficiary.AccountNumber = aItem["AccountNumber"].ToString() ?? "";
                beneficiary.AccountType = aItem["AccountType"].ToString() ?? "";
                beneficiary.CheckPayableTo = aItem["CheckPayableTo"].ToString() ?? "";
                beneficiary.FirstName = aItem["FirstName"].ToString() ?? "";
                beneficiary.LastName = aItem["LastName"].ToString() ?? "";
                beneficiary.AddressLine1 = aItem["AddressLine1"].ToString() ?? "";
                beneficiary.AddressLine2 = aItem["AddressLine2"].ToString() ?? "";
                beneficiary.City = aItem["City"].ToString() ?? "";
                beneficiary.State = aItem["State"].ToString() ?? "";
                beneficiary.Zip = Convert.ToInt32(Convert.IsDBNull(aItem["Zip"]) ? 0 : aItem["Zip"]);
                lstBeneficiary.Add(beneficiary);
            }
            return lstBeneficiary;
        }
        public async Task<Response<BeneficiaryDetails>> GetBeneficiaryByCustomerId(string customerId)
        {
            Response<BeneficiaryDetails> response = new Response<BeneficiaryDetails>();
            try
            {
                MySqlCommand cmdBeneficiary = new MySqlCommand(StoredProcedures.CUSTOMER_GET_ALL_BENEFICIARIES);
                cmdBeneficiary.CommandType = CommandType.StoredProcedure;

                cmdBeneficiary.Parameters.AddWithValue("@p_CustomerId", customerId);

                DataTable tblBeneficairies = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdBeneficiary));

                response.StatusCode = 200;
                response.Message = "Beneficiari(es) fetched Successfully.";
                response.Result = MapDataTableToBeneficiaryList(tblBeneficairies);
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

        public async Task<Response<CustomerAuditHistory>> ActivateDeactivateHistoryByCustomerId(string customerId)
        {
            Response<CustomerAuditHistory> response = new Response<CustomerAuditHistory>();
            try
            {
                MySqlCommand cmdBeneficiary = new MySqlCommand(StoredProcedures.CUSTOMER_GETALLACTIVATEDEACTIVATEHISTORY);
                cmdBeneficiary.CommandType = CommandType.StoredProcedure;

                cmdBeneficiary.Parameters.AddWithValue("@p_CustomerId", customerId);

                DataTable tblBeneficairies = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdBeneficiary));

                response.StatusCode = 200;
                response.Message = "Activate/Deactivate History fetched Successfully.";
                response.Result = MapDataTableToCustomerAuditHistory(tblBeneficairies);
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
        private List<CustomerAuditHistory> MapDataTableToCustomerAuditHistory(DataTable tblBeneficiary)
        {
            List<CustomerAuditHistory> lstBeneficiary = new List<CustomerAuditHistory>();
            foreach (DataRow aItem in tblBeneficiary.Rows)
            {
                CustomerAuditHistory custaudithistory = new CustomerAuditHistory();
                custaudithistory.CustomerAuditHistoryId = aItem["CustomerAuditHistoryId"].ToString() ?? "";
                custaudithistory.CustomerId = aItem["CustomerId"].ToString() ?? "";
                custaudithistory.Comments = aItem["Comments"].ToString() ?? "";
                custaudithistory.Action = aItem["Action"].ToString() ?? "";
                custaudithistory.AuditDate =Convert.ToDateTime(aItem["AuditDate"].ToString() ?? "");

                lstBeneficiary.Add(custaudithistory);
            }
            return lstBeneficiary;
        }
    }
}
