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
    }
}
