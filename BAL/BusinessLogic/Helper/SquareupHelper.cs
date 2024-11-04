using BAL.BusinessLogic.Interface;
using BAL.Models.SquareupPayments;
using BAL.ResponseModels;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Square;
using Square.Authentication;
using Square.Exceptions;
using Square.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BAL.BusinessLogic.Helper
{
    public class SquareupHelper :ISquareupHelper
    {
        private readonly string _accessToken;
        private readonly string _environment;
        private BearerAuthModel _bearerAuthModel;
        public Square.Environment SquareupEnvironment
        {
            get {
                switch(_environment.ToUpper())
                {
                    case "SANDBOX":
                        return Square.Environment.Sandbox;
                    case "PRODUCTION": 
                        return Square.Environment.Production;
                    default:
                        return Square.Environment.Sandbox;
                }
            }
        }
        public SquareClient SquareupClient
        {
            get
            {
                return new SquareClient.Builder()
                .Environment(SquareupEnvironment)
                .BearerAuthCredentials(_bearerAuthModel)
                .Build();
            }
        }
        public SquareupHelper(IConfiguration configuration) { 
            _accessToken = configuration?.GetSection("SquareUp")["access_token"] ?? "";
            _environment = configuration?.GetSection("SquareUp")["Environment"] ?? "";

            _bearerAuthModel = new BearerAuthModel.Builder(_accessToken).Build();
        }

        public async Task<Response<Models.SquareupPayments.SquareupPaymentResponse>> ProcessPaymentRequest(SquareupPaymentRequest request)
        {
            var response = new Response<Models.SquareupPayments.SquareupPaymentResponse>();
            var amountMoney = new Money.Builder()
                .Amount(request.Amount)
                .Currency(request.Currency)
                .Build();

            string idempotencyKey = Guid.NewGuid().ToString();

            var createPaymentRequest = new CreatePaymentRequest.Builder(
                sourceId: request.SourceId,
                idempotencyKey: idempotencyKey)
                .AmountMoney(amountMoney)
                .Build();

            try
            {
                var pmtResponse = await SquareupClient.PaymentsApi.CreatePaymentAsync(createPaymentRequest);
                if (pmtResponse.Errors != null && pmtResponse.Errors.Count > 0)
                {
                    if (pmtResponse.Errors.Any(e => e.Code == "CARD_TOKEN_USED"))
                    {
                        response.StatusCode = 400;
                        response.Message = "Please generate a new payment nonce for this transaction.";
                    }
                    response.StatusCode = 400;
                    response.Message = $"Payment processing failed: {string.Join(", ", pmtResponse.Errors.Select(e => e.Detail))}";
                }
                var paymentResult = pmtResponse.Payment;

                var paymentResponse = new Models.SquareupPayments.SquareupPaymentResponse
                {
                    Id = paymentResult.Id,
                    Amount = paymentResult.AmountMoney.Amount,
                    Currency = paymentResult.AmountMoney.Currency,
                    Status = paymentResult.Status,
                    ReceiptUrl = paymentResult.ReceiptUrl,
                    CardBrand = paymentResult.CardDetails.Card.CardBrand,
                    Last4 = paymentResult.CardDetails.Card.Last4,
                    Expiration = $"{paymentResult.CardDetails.Card.ExpMonth}/{paymentResult.CardDetails.Card.ExpYear}"
                };

                response.StatusCode = 200;
                response.Message = "Payment Successfully Processed.";
                response.Result = new List<Models.SquareupPayments.SquareupPaymentResponse>() { paymentResponse };
            }
            catch (ApiException apiEx)
            {
                if (apiEx.Errors != null && apiEx.Errors.Count > 0)
                {
                    var errorDetails = string.Join(", ", apiEx.Errors.Select(e => e.Detail));

                    response.StatusCode = 500;
                    response.Message = $"Payment processing failed: {errorDetails}";                    
                }
                response.StatusCode = 500;
                response.Message = $"Payment processing failed: {apiEx.Message}";                
            }
            catch (Exception ex)
            {       
                response.StatusCode = 500;
                response.Message = $"Payment processing failed: {ex.Message}";                
            }
            return response;
        }
    }
}
