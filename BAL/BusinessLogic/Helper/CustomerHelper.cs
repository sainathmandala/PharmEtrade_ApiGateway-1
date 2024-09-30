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

        public CustomerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, SmtpSettings smtpSettings , IJwtHelper jwtHelper, IEmailHelper emailHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
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
                        //string _mailBody = EmailTemplates.CUSTOMER_TEMPLATE;
                        //_mailBody = _mailBody.Replace("{{CustomerId}}", customer.CustomerId);
                        //_mailBody = _mailBody.Replace("{{RegistrationDetailsHTML}}", GetCustomerDetailsHTml(customer));
                        //await _emailHelper.SendEmail(customer.Email, "", " Registration is completed  Successfully ", _mailBody);
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
