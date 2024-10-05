using BAL.ResponseModels;
using BAL.Models;
using BAL.RequestModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IPaymentinfoRepository
    {


        Task<BAL.ResponseModels.Response<PaymentInfo>> AddPayment(PaymentInfo paymentInfo);
        Task<BAL.ResponseModels.Response<PaymentInfo>> UpdatePayment(PaymentInfo paymentInfo);
        Task<Response<PaymentInfo>> GetPaymentInfoByOrderId(string OrderId);
        Task<Response<PaymentInfo>> GetPaymentInfoByCustmoerId(string CustomerId);
        Task<Response<PaymentInfo>> GetAllPayments();
        Task<Response<PaymentInfo>> GetAllPayments(PaymentCriteria criteria);
    }
}
