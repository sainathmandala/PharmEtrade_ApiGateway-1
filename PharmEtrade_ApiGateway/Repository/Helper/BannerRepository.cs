using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class BannerRepository : IBannerRepository
    {
        private readonly IBannerHelper bannerHelper;
        public BannerRepository(IBannerHelper bannerHelper)
        {
            this.bannerHelper = bannerHelper;
        }

        public async Task<Response<Banner>> AddUpdateBanner(Banner banner)
        {
            return await bannerHelper.AddUpdateBanner(banner);
        }

        public async Task<Response<Banner>> DeleteBanner(int bannerId)
        {
            return await bannerHelper.DeleteBanner(bannerId);
        }

        public async Task<Response<Banner>> GetBanners()
        {
            return await bannerHelper.GetBanners();
        }
    }
}
