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
        public const string GET_PRODUCTS_BY_SELLER = "sp_GetProductsBySeller";
        public const string GET_PRODUCTS_BY_CRITERIA = "sp_GetProductsByCriteria";

        // ORDERS
        public const string GET_ORDERS = "sp_GetOrders";
        public const string ADD_UPDATE_ORDER = "sp_InsertOrder";
        public const string GET_ORDERS_BY_SELLER = "sp_GetOrdersByVendorId";

        //Cart
        public const string DELETE_CART = "sp_DeleteCart";


        // CUSTOMERS

        // WISHLIST
        public const string DELETE_WishList = "sp_RemoveWishlist";


        // MENU

        // BANNER
        public const string ADD_UPDATE_BANNER = "sp_AddUpdateBanner";
        public const string GET_BANNERS = "sp_GetBanners";
        public const string DELETE_BANNER = "sp_DeleteBanner";

        // MASTERS
        public const string GET_NDCUPC_DETAILS = "sp_GetNDCUPCDetails";
    }
}
