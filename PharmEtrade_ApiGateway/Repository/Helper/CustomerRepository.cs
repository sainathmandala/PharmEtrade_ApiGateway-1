using BAL.BusinessLogic.Interface;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class CustomerRepository:IcustomerRepo
    {
        private readonly IcustomerHelper _icustomerHelper;
        public CustomerRepository(IcustomerHelper icustomerHelper)
        {
            _icustomerHelper = icustomerHelper;
        }


        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for customer login 
        public async Task<string> CustomerLogin(string username, string password)
        {
            //AdminDetailsResponse response = new AdminDetailsResponse();
            //try
            //{
            //    DataTable dtresult = await _iAdminHelper.LoginAdmin(username, password);
            //    if (dtresult != null && dtresult.Rows.Count > 0)
            //    {
            //        response.statusCode = 100;
            //        response.message = Constant.adminloginSucessMsg;
            //        response.adminlist = ConvertDataTabletoList(dtresult);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.statusCode = 102;
            //    response.message = ex.Message;
            //    response.adminlist = new List<Admin>();

            //}
            //return response;
            return null;
        }
    }
}
