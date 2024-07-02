using BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IcustomerHelper
    {
        Task<DataTable> CustomerLogin(string username, string password);
        Task<int> AddToCart(int userId, int imageId, int productId);
        Task<int> dummy(int userId, int imageId, int productId);
        Task<string> SaveCustomerData(UserViewModel userView);
        Task<DataTable> GetUserDetailsById(int userId);
    }
}
