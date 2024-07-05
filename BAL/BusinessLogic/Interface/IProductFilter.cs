using BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IProductFilter
    {
        Task<DataTable> GetFilteredProducts(int? productCategoryId, string productName);
        Task<DataTable> GetProducts();

        Task<DataTable> GetProductsById(int AddproductID);

    }
}
