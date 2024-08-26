using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Common
{
    public static class StoredProcedures
    {
        // PRODUCTS
        public const string ADD_UPDATE_PRODUCT = "sp_AddUpdateProduct";
        public const string ADD_UPDATE_PRODUCTSIZE = "sp_AddUpdateProductSize";
        public const string GET_PRODUCTS = "sp_GetProducts";
        public const string GET_PRODUCTS_BY_SPECIFICATION = "sp_GetProductsBySpecification";
        public const string GET_RECENT_SOLD_PRODUCTS = "sp_GetRecentSoldProducts";

        // ORDERS
        public const string GET_ORDERS = "sp_GetOrders";
        public const string ADD_UPDATE_ORDER = "sp_InsertOrder";
        public const string GET_ORDERS_BY_SELLER = "sp_GetOrdersByVendorId";

        // CUSTOMERS

        // WISHLIST

        // MENU

        // BANNER
        public const string ADD_UPDATE_BANNER = "sp_AddUpdateBanner";
        public const string GET_BANNERS = "sp_GetBanners";
        public const string DELETE_BANNER = "sp_DeleteBanner";
    }
}
