using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IMastersRepository
    {
        Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC);
    }
}
