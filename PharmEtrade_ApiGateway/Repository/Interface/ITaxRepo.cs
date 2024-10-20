using BAL.Models;
using BAL.RequestModels;
using BAL.RequestModels.Customer;
using BAL.ResponseModels;
using BAL.ViewModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface ITaxRepo
    {

        Task<Response<TaxInformation>> GetAllTaxInformation();
        Task<Response<TaxInformation>> AddTaxInformationDetails(TaxInformation taxInformation);
        Task<Response<TaxInformation>> GetTaxInformationByStateName(string stateName);
        Task<Response<TaxInformation>> GetTaxInformationByCategorySpecificationId(int categorySpecificationId);
        Task<Response<TaxInformation>> UpdateTaxInformation(TaxInformation taxInformation);

    }
}
