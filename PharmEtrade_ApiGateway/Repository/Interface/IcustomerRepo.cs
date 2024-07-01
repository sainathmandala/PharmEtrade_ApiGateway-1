namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IcustomerRepo
    {
        Task<string> CustomerLogin(string username, string password);
        Task<int> AddToCart(int userId, int imageId, int productId);
    }
}
