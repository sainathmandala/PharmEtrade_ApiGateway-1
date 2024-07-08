using BAL.ViewModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IcustomerRepo
    {
        Task<loginViewModel> CustomerLogin(string username, string password);
        Task<int> AddToCart(int userId, int imageId, int productId);
        Task<Response> UserRegistration(UserViewModel userViewModel);
        Task<UserDetailsResponse> GetUserDetailsByUserId(int userId);
        Task<Response> UpdatePassword(int id,string newPassword);
        Task<UserEmailResponse> GetUserDetailsByEmail(string email);
        //this method only in this not BAL
        Task<UserEmailViewModel> ForgotPassword(string email);
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task<Response> UpdatePasswordByEmail(string email, string newPassword);
    }
}
