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
        public const string ADD_UPDATE_PRODUCT_INFO = "sp_AddUpdateProductInfo";
        public const string ADD_UPDATE_PRODUCT_PRICE = "sp_AddUpdateProductPrice";
        public const string ADD_UPDATE_PRODUCT_GALLERY = "sp_AddUpdateProductGallery";
        public const string ADD_UPDATE_PRODUCTSIZE = "sp_AddUpdateProductSize";

        public const string GET_ALL_PRODUCTS = "sp_GetAllProducts";
        public const string GET_PRODUCTS_BY_SPECIFICATION = "sp_GetProductsBySpecification";
        public const string GET_RECENT_SOLD_PRODUCTS = "sp_GetRecentSoldProducts";
        public const string GET_PRODUCTS_BY_SELLER = "sp_GetProductsBySeller";
        public const string GET_PRODUCTS_BY_CRITERIA = "sp_GetProductsByCriteria";

        // ORDERS
        public const string ADD_UPDATE_ORDER = "sp_InsertOrder";

        public const string GET_ORDERS = "sp_GetOrders";
        public const string GET_ORDERS_BY_SELLER = "sp_GetOrdersByVendorId";
        public const string ADD_PAYMENT = "sp_AddPayment";

        //Cart
        public const string DELETE_CART = "sp_DeleteCart";

        // CUSTOMERS
        public const string CUSTOMER_ADD_UPDATE_ADDRESS = "sp_AddUpdateAddress";
        public const string CUSTOMER_DELETE_ADDRESS = "sp_DeleteAddress";

        public const string CUSTOMER_GET_ALL_ADDRESSES = "sp_GetAddressByCustomerId";
        public const string CUSTOMER_GET_ADDRESS = "sp_GetAddress";

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
