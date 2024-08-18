using BAL.RequestModels;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Models;

namespace BAL.BusinessLogic.Interface
{
    public interface IOrders
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByCustomerId(string customerId);

    }
}
