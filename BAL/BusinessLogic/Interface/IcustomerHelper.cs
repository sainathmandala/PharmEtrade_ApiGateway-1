using BAL.ResponseModels;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IcustomerHelper
    {
        Task<DataTable> CustomerLogin(string username, string password);
        Task<int> AddToCart(int userId, int imageId, int productId);
        Task<int> dummy(int userId, int imageId, int productId);
        Task<string> SaveCustomerData(UserViewModel userView);
        Task<DataTable> GetUserDetailsById(int userId);
        Task<string> UpdatePassword(int userId,string password);
        Task<DataTable> GetUserDetailsByEmail(string email);
        Task<string> UpdatePasswordByEmail(string email, string password);
        Task<string> SendOTPEmail(string email);
        Task<DataTable> OtpLogin(string email, string otp);
        Task<string> SaveBusinessInfoData(BusinessInfoViewModel businessInfo);
        Task<string> AddUpdateCustomer(Customer customer);
        Task<string> AddUpdateBusinessInfo(CustomerBusinessInfo businessInfo);
        Task<UploadResponse> UploadImage(IFormFile image);

        Task<Response<CustomerResponse>> GetCustomerByCustomerId(string customerId);
    }
}
