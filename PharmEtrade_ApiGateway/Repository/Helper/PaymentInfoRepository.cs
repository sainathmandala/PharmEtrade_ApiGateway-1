using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.BusinessLogic.Helper;
using Mysqlx.Crud;



namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class PaymentInfoRepository : IPaymentinfoRepository
    {
        private readonly IPaymentInfo _paymentInfohelper;

        public PaymentInfoRepository(IPaymentInfo paymentInfo)
        {
            _paymentInfohelper = paymentInfo;
        }


        public async Task<Response<PaymentInfo>> AddPayment(PaymentInfo paymentInfo)
        {

            return await _paymentInfohelper.AddPayment(paymentInfo);
        }

       
        public async  Task<Response<PaymentInfo>> GetPaymentInfoByCustmoerId(string CustomerId)
        {

            return await _paymentInfohelper.GetPaymentInfoByCustmoerId(CustomerId);

        }

        public async  Task<Response<PaymentInfo>> GetPaymentInfoByOrderId(string OrderId)
        {
            return await _paymentInfohelper.GetPaymentInfoByOrderId(OrderId);

        }

        
        public async Task<Response<PaymentInfo>> UpdatePayment(PaymentInfo paymentInfo)
        {
            return await _paymentInfohelper.UpdatePayment(paymentInfo);
        }
    }
}
