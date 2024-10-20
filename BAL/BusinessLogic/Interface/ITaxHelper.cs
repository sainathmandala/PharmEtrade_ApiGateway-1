using BAL.Models;
using BAL.RequestModels;
using BAL.RequestModels.Customer;
using BAL.ResponseModels;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface ITaxHelper
    {
        Task<Response<TaxInformation>> GetAllTaxInformation();
        Task<Response<TaxInformation>> AddTaxInformationDetails(TaxInformation taxInformation);
        Task<Response<TaxInformation>> GetTaxInformationByStateName(string stateName);
        Task<Response<TaxInformation>> GetTaxInformationByCategorySpecificationId(int categorySpecificationId);
        Task<Response<TaxInformation>> UpdateTaxInformation(TaxInformation taxInformation);
    }
}
