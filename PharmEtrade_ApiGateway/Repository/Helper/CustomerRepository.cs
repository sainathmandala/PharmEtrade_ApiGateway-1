using BAL.BusinessLogic.Helper;
using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;
using PharmEtrade_ApiGateway.Extensions;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class CustomerRepository:IcustomerRepo
    {
        private readonly IcustomerHelper _icustomerHelper;
        private readonly JwtAuthenticationExtensions _jwtTokenService;
        public CustomerRepository(IcustomerHelper icustomerHelper, JwtAuthenticationExtensions jwtTokenService)
        {
            _icustomerHelper = icustomerHelper;
            _jwtTokenService = jwtTokenService;
        }

        public Task<int> AddToCart(int userId, int imageId, int productId)
        {
            throw new NotImplementedException();
        }


        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for customer login 
        public async Task<loginViewModel> CustomerLogin(string username, string password)
        {
            loginViewModel response = new loginViewModel();

            try
            {
                var dtResult = await _icustomerHelper.CustomerLogin(username, password);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow row = dtResult.Rows[0];
                    response.LoginStatus = row["LoginStatus"].ToString();
                    response.UserId = Convert.ToInt32(row["UserId"]);
                    response.Username = row["Username"].ToString();
                    response.UserEmail = row["UserEmail"].ToString();
                    response.Role = row["Role"].ToString();

                    if (response.LoginStatus == "Success")
                    {
                        response.token = _jwtTokenService.GenerateToken(response.Username, response.Role);
                        response.statusCode = 200;
                        //response.message = LoginSuccessMsg;
                    }
                    else
                    {
                        response.statusCode = 400;
                        response.message = "Invalid credentials or user role not found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = $"An error occurred: {ex.Message}";
            }

            return response;

        }

      
    }
}
