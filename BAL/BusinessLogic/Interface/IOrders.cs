using BAL.RequestModels;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.ViewModels;

namespace BAL.BusinessLogic.Interface
{
    public interface IOrders
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest);
        Task<List<Order>> GetOrdersByCustomerId(string customerId);

    }
}
