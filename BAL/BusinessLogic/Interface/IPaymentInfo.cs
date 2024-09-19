using BAL.Models;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IPaymentInfo
    {
        Task<BAL.ResponseModels.Response<PaymentInfo>> AddPayment(PaymentInfo paymentInfo);
        Task<BAL.ResponseModels.Response<PaymentInfo>>UpdatePayment(PaymentInfo paymentInfo);
        Task<Response<PaymentInfo>> GetPaymentInfoByOrderId(string OrderId);
        Task<Response<PaymentInfo>> GetPaymentInfoByCustmoerId(string CustomerId);


    }
}
