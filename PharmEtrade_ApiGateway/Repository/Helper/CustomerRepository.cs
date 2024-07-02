using BAL.BusinessLogic.Helper;
using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;
using PharmEtrade_ApiGateway.Extensions;
using DAL.Models;

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
        // Author: [shiva]
        // Created Date: [02/07/2024]
        // Description: Method for registration of User
        public async Task<Response> UserRegistration(UserViewModel userViewModel)
        {
            Response response = new Response();
            try
            {
                string status = await _icustomerHelper.SaveCustomerData(userViewModel);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.UserCreationSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }


        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for Get the data of User Based On UserId
        public async Task<UserDetailsResponse> GetUserDetailsByUserId(int userId)
        {
            UserDetailsResponse response = new UserDetailsResponse();
            try
            {
                DataTable dtresult = await _icustomerHelper.GetUserDetailsById(userId);
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    response.statusCode = 200;
                    response.message = Constant.GetUserBYUserIdSuccessMsg;
                    response.userlist = ConvertDataTabletoStudentList(dtresult);
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = ex.Message;
                response.userlist = new List<UserViewModel>();

            }
            return response;
        }

        private List<UserViewModel> ConvertDataTabletoStudentList(DataTable dt)
        {
            List<UserViewModel> userlst = new List<UserViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    UserViewModel user = new UserViewModel();
                    user.UserId = Convert.ToInt32(row["user_id"]);
                    user.Username = row["username"].ToString();
                    user.Email = row["email"].ToString();
                    user.Password = row["password"].ToString();
                    user.PhoneNumber =row["phone_number"].ToString();
                  

                    userlst.Add(user);

                }
                return userlst;
            }
            else
                return userlst;
        }
    }
}
