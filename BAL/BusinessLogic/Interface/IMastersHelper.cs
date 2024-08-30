using BAL.Models;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IMastersHelper
    {
        Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC);
    }
}
