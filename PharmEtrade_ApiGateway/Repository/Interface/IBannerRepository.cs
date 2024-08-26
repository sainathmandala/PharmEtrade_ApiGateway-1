using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IBannerRepository
    {
        Task<Response<Banner>> GetBanners();
        Task<Response<Banner>> AddUpdateBanner(Banner banner);
        Task<Response<Banner>> DeleteBanner(int bannerId);
    }
}
