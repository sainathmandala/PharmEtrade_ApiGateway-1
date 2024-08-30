using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class MastersRepository : IMastersRepository
    {
        private readonly IMastersHelper _mastersHelper;
        public MastersRepository(IMastersHelper mastersHelper)
        {
            _mastersHelper = mastersHelper;
        }

        public async Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC)
        {
            return await _mastersHelper.GetNDCUPCDetails(NDC, UPC);
        }
    }
}
