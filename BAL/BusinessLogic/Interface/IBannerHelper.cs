using BAL.Models;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IBannerHelper
    {
        Task<Response<Banner>> GetBanners();
        Task<Response<Banner>> AddUpdateBanner(Banner banner);
        Task<Response<Banner>> DeleteBanner(int bannerId);
    }
}
