using BAL.Models.FedEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IFedExHelper
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
    }
}
