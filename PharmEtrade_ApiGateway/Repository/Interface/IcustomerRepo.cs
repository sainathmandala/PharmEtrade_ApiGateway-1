using BAL.Models;
using BAL.RequestModels;
using BAL.RequestModels.Customer;
using BAL.ResponseModels;
using BAL.ViewModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface ICustomerRepo
    {
        Task<LoginViewModel> CustomerLogin(string username, string password);
        Task<LoginResponse> AdminLogin(string adminId, string password);        
        Task<RegistrationResponse> RegisterCustomer(CustomerAddRequest customer);
        Task<UploadResponse> UploadImage(IFormFile image);
        Task<BusinessInfoResponse> AddUpdateBusinessInfo(CustomerBusinessInfo businessInfo);
        Task<Response<CustomerResponse>> GetCustomerByCustomerId(string customerId);
        Task<Response<Customer>> GetCustomers(string? customerId, string? email, string? mobile);
        Task<Response<Customer>> GetByFilterCriteria(CustomerFilterCriteria filterCriteria);
        Task<Response<Address>> GetByCustomerId(string customerId);
        Task<Response<Address>> GetAddressById(string addressId);
        Task<Response<Address>> AddUpdateAddress(Address customerAddress);
        Task<Response<Address>> DeleteAddress(string addressId);
        Task<Response<string>> Activate(string customerId, string? comments);
        Task<Response<string>> Deactivate(string customerId, string? comments);


        Task<Response<BeneficiaryDetails>> AddUpdateBeneficiaryDetail(BeneficiaryDetails beneficiaryDetails);
        Task<Response<BeneficiaryDetails>> GetBeneficiaryByCustomerId(string customerId);
    }
}
