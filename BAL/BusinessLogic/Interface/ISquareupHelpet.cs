using BAL.Models.SquareupPayments;
using BAL.ResponseModels;
using Square;
using Square.Models;

namespace BAL.BusinessLogic.Interface
{
    public interface ISquareupHelper
    {
        Square.Environment SquareupEnvironment { get; }
        SquareClient SquareupClient { get; }
        Task<Response<Models.SquareupPayments.SquareupPaymentResponse>> ProcessPaymentRequest(SquareupPaymentRequest request);
    }
}
