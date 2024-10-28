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
using BAL.ResponseModels;
using BAL.Models;
using ZstdSharp.Unsafe;
using BAL.RequestModels;
using BAL.RequestModels.Customer;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class CustomerRepository : ICustomerRepo
    {
        private readonly ICustomerHelper _icustomerHelper;
        private readonly JwtAuthenticationExtensions _jwtTokenService;

        private readonly IConfiguration _configuration;
        private readonly SmtpSettings _smtpSettings;

        public CustomerRepository(ICustomerHelper icustomerHelper, JwtAuthenticationExtensions jwtTokenService, IConfiguration configuration, IOptions<SmtpSettings> smtpSettings)
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

        public async Task<LoginViewModel> CustomerLogin(string username, string password)
        {
            LoginViewModel response = new LoginViewModel();
            try
            {
                var dtResult = await _icustomerHelper.CustomerLogin(username, password);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow row = dtResult.Rows[0];
                    response.LoginStatus = row["LoginStatus"].ToString() ?? "";
                    response.UserId = row["UserId"].ToString() ?? "";
                    response.Firstname = row["Firstname"].ToString() ?? "";
                    response.Lastname = row["Lastname"].ToString() ?? "";
                    response.UserEmail = row["UserEmail"].ToString() ?? "";
                    response.UserType = row["userType"].ToString() ?? "";
                    response.IsActive = Convert.ToInt32(Convert.IsDBNull(row["IsActive"]) ? 0 : row["IsActive"]) == 1;

                    if (response.LoginStatus == "Success" && response.IsActive)
                    {
                        response.Token = _jwtTokenService.GenerateToken(response.UserEmail, response.UserType);
                        response.statusCode = 200;
                        //response.message = LoginSuccessMsg;
                    }
                    else if(response.LoginStatus == "Success" && !response.IsActive)
                    {
                        response.statusCode = 400;
                        response.Message = "User is not Active. Please contact Administrator";
                    }
                    else
                    {
                        response.statusCode = 400;
                        response.Message = "Invalid credentials or user role not found";
                    }
                }
                else
                {
                    response.statusCode = 400;
                    response.Message = "Invalid credentials or user role not found";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = $"An error occurred: {ex.Message}";
            }
            return response;
        }

        public async Task<LoginResponse> AdminLogin(string adminId, string password)
        {
            if(string.IsNullOrEmpty(adminId) || string.IsNullOrEmpty(password))
            {
                return new LoginResponse() { StatusCode = 400, Message = "Admin Id and Password are required."};
            }
            return await _icustomerHelper.AdminLogin(adminId, password);
        }

        public async Task<RegistrationResponse> RegisterCustomer(CustomerAddRequest customer)
        {
            RegistrationResponse response = new RegistrationResponse();
            try
            {
                string result = await _icustomerHelper.AddCustomer(customer);
                if (!result.StartsWith("ERROR"))
                {
                    response.Status = 200;
                    response.CustomerId = result;
                    response.Message = Constant.UserCreationSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;

            }
            return response;
        }

        public async Task<RegistrationResponse> EditCustomer(CustomerEditRequest customer)
        {
            RegistrationResponse response = new RegistrationResponse();
            try
            {
                string result = await _icustomerHelper.EditCustomer(customer);
                if (!result.StartsWith("ERROR"))
                {
                    response.Status = 200;
                    response.CustomerId = result;
                    response.Message = Constant.UserUpdationSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;

            }
            return response;
        }

        public async Task<UploadResponse> UploadImage(IFormFile image)
        {
            UploadResponse response = new UploadResponse();
            try
            {
                response = await _icustomerHelper.UploadImage(image);
            }
            catch (Exception ex)
            {
                //response.Status = 500;
                //response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BusinessInfoResponse> AddUpdateBusinessInfo(CustomerBusinessInfo businessInfo)
        {
            BusinessInfoResponse response = new BusinessInfoResponse();
            response.CustomerId = businessInfo.CustomerId ?? "";
            try
            {
                string result = await _icustomerHelper.AddUpdateBusinessInfo(businessInfo);
                if (!result.StartsWith("ERROR"))
                {
                    response.Status = 200;
                    response.Message = Constant.BusinessInfoSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;

            }
            return response;
        }

        public async Task<Response<CustomerResponse>> GetCustomerByCustomerId(string customerId)
        {
            var response = new Response<CustomerResponse>();
            if (string.IsNullOrEmpty(customerId))
            {
                response.StatusCode = 400;
                response.Message = "Bad Request : Customer Id is not provided.";
                response.Result = null;
            }
            return await _icustomerHelper.GetCustomerByCustomerId(customerId);
        }

        public async Task<Response<Customer>> GetCustomers(string? customerId, string? email, string? mobile)
        {
            return await _icustomerHelper.GetCustomers(customerId, email, mobile);
        }

        public async Task<Response<CustomerAddress>> GetByCustomerId(string customerId)
        {
            return await _icustomerHelper.GetByCustomerId(customerId);
        }

        public async Task<Response<CustomerAddress>> GetAddressById(string addressId)
        {
            return await _icustomerHelper.GetAddressById(addressId);
        }

        public async Task<Response<CustomerAddress>> AddUpdateAddress(CustomerAddress customerAddress)
        {
            return await _icustomerHelper.AddUpdateAddress(customerAddress);
        }

        public async Task<Response<CustomerAddress>> DeleteAddress(string addressId)
        {
            return await _icustomerHelper.DeleteAddress(addressId);
        }

        public async Task<Response<Customer>> GetByFilterCriteria(CustomerFilterCriteria filterCriteria)
        {
            return await _icustomerHelper.GetByFilterCriteria(filterCriteria);
        }

        public async Task<Response<string>> Activate(string customerId, string? comments)
        {
            return await _icustomerHelper.Activate(customerId, comments);
        }

        public async Task<Response<string>> Deactivate(string customerId, string? comments)
        {
            return await _icustomerHelper.Deactivate(customerId, comments);
        }
        public async Task<Response<BeneficiaryDetails>> AddUpdateBeneficiaryDetail(BeneficiaryDetails beneficiaryDetails)
        {
            return await _icustomerHelper.AddUpdateBeneficiaryDetail(beneficiaryDetails);
        }
        public async Task<Response<BeneficiaryDetails>> GetBeneficiaryByCustomerId(string customerId)
        {
            return await _icustomerHelper.GetBeneficiaryByCustomerId(customerId);
        }
        public async Task<Response<CustomerAuditHistory>> ActivateDeactivateHistoryByCustomerId(string customerId)
        {
            return await _icustomerHelper.ActivateDeactivateHistoryByCustomerId(customerId);
        }
        public async Task<string> DeleteCustomer(string customerId)
        {
            return await _icustomerHelper.DeleteCustomer(customerId);
        }
        public async Task<Response<string>> SendChangePasswordLink(string customerId)
        {
            return await _icustomerHelper.SendChangePasswordLink(customerId);
        }

        public async Task<Response<Customer>> ChangePassword(string customerId, string newPassword)
        {
            return await _icustomerHelper.ChangePassword(customerId, newPassword);
        }
    }
}
