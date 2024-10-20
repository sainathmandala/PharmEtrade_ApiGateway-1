using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.ViewModels;
using Microsoft.Extensions.Options;
using PharmEtrade_ApiGateway.Extensions;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class TaxRepository : ITaxRepo
    {
        private readonly ITaxHelper _itaxHelper;
        private readonly JwtAuthenticationExtensions _jwtTokenService;

        private readonly IConfiguration _configuration;
        private readonly SmtpSettings _smtpSettings;

        public TaxRepository(ITaxHelper itaxHelper, JwtAuthenticationExtensions jwtTokenService, IConfiguration configuration, IOptions<SmtpSettings> smtpSettings)
        {
            _itaxHelper = itaxHelper;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _smtpSettings = smtpSettings.Value;
        }
        public async Task<Response<TaxInformation>> AddTaxInformationDetails(TaxInformation taxInformation)
        {

            return await _itaxHelper.AddTaxInformationDetails(taxInformation);
        }

        public async Task<Response<TaxInformation>> GetAllTaxInformation()
        {
            return await _itaxHelper.GetAllTaxInformation();
        }

        public async Task<Response<TaxInformation>> GetTaxInformationByCategorySpecificationId(int categorySpecificationId)
        {
            return await _itaxHelper.GetTaxInformationByCategorySpecificationId(categorySpecificationId);
        }

        public async Task<Response<TaxInformation>> GetTaxInformationByStateName(string stateName)
        {
            return await _itaxHelper.GetTaxInformationByStateName(stateName);
        }

        public async Task<Response<TaxInformation>> UpdateTaxInformation(TaxInformation taxInformation)
        {
            return await _itaxHelper.UpdateTaxInformation(taxInformation);
        }
    }
}
