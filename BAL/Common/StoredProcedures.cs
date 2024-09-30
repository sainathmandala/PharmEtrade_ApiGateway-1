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
        public const string ADD_RELATED_PRODUCT = "sp_AddRelatedproduct";
        public const string ADD_UPSELL_PRODUCT = "sp_AddUpsellProduct";
        public const string ADD_CROSS_SELL_PRODUCT = "sp_AddCrossSellProduct";

        public const string REMOVE_RELATED_PRODUCT = "sp_RemoveRelatedproduct";
        public const string REMOVE_UPSELL_PRODUCT = "sp_RemoveUpsellProduct";
        public const string REMOVE_CROSS_SELL_PRODUCT = "sp_RemoveCrossSellProduct";

        public const string GET_ALL_PRODUCTS = "sp_GetAllProducts";
        public const string GET_PRODUCTS_BY_SPECIFICATION = "sp_GetProductsBySpecification";
        public const string GET_RECENT_SOLD_PRODUCTS = "sp_GetRecentSoldProducts";
        public const string GET_PRODUCTS_BY_SELLER = "sp_GetProductsBySeller";
        public const string GET_PRODUCTS_BY_CRITERIA = "sp_GetProductsByCriteria";
        public const string GET_RELATED_PRODUCTS = "sp_GetRelatedproducts";
        public const string GET_UPSELL_PRODUCTS = "sp_GetUpsellProducts";
        public const string GET_CROSS_SELL_PRODUCTS = "sp_GetCrossSellProducts";
        public const string PRODUCTS_GET_PRODUCTS_COUNT_BY_CATEGORY = "sp_GetProductsCountByCategory";
        public const string PRODUCT_DEACTIVE = "sp_ProductDeactivate";
        public const string PRODUCT_DELETE = "sp_DeleteProduct";


        //PRODUCT RATING
        public const string ADD_PRODUCT_RATING = "sp_AddUpdateProductRating";
        public const string UPDATE_PRODUCT_RATING = "sp_AddUpdateProductRating";

        public const string REMOVE_PRODUCT_RATING = "sp_RemoveRating";

        public const string GET_PRODUCT_RATING = "sp_GetRatingsWithProduct";
        public const string GET_PRODUCT_RATING_BYID = "sp_GetRatingById";



        // ORDERS
        public const string ADD_UPDATE_ORDER = "sp_AddUpdateOrder";

        public const string GET_ORDERS = "sp_GetOrders";
        public const string GET_ORDERS_BY_SELLER = "sp_GetOrdersByVendorId";
        public const string ADD_PAYMENT = "sp_AddPayment";
        public const string GET_ORDERS_BY_ORDERID = "Sp_GetOrdersByOrderId";
        public const string GETCUSTOMERORDEREDFORSELLER = "sp_GetCustomersOrderedForSeller";
        public const string GET_ORDER_INVOICE = "sp_OrderInvoice";

        //Cart
        public const string DELETE_CART = "sp_DeleteCart";

        // CUSTOMERS
        public const string CUSTOMER_ADMIN_LOGIN = "sp_AdminLogin";
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
        public const string MASTERS_GET_NDCUPC_DETAILS = "sp_GetNDCUPCDetails";

        public const string MASTERS_GET_PRODUCT_CATEGORIES = "sp_GetProductCategories";
        public const string MASTERS_ADD_UPDATE_PRODUCT_CATEGORY = "sp_AddUpdateProductCategory";
        public const string MASTERS_REMOVE_PRODUCT_CATEGORY = "sp_RemoveProductCategory";

        public const string MASTERS_GET_CATEGORY_SPECIFICATION = "sp_GetCategoriesSpecification";
        public const string MASTERS_ADD_UPDATE_CATEGORY_SPECIFICATION = "sp_AddUpdateCategoriesSpecification";
        public const string MASTERS_REMOVE_CATEGORY_SPECIFICATION = "sp_RemoveCategoriesSpecification";

        // BIDS
        public const string GET_BIDS_BY_BUYER = "sp_GetBidsByBuyer";
        public const string GET_BIDS_BY_SELLER = "sp_GetBidsBySeller";
        public const string GET_BIDS_BY_PRODUCT = "sp_GetBidsByProduct";
        public const string GET_PRODUCTS_QUOTED_BY_BUYER = "sp_GetProductsQuotedByBuyer";
        public const string GET_PRODUCTS_QUOTED_BY_SELLER = "sp_GetProductsQuotedBySeller";

        public const string ADD_UPDATE_BID = "sp_AddUpdateBid";        
        public const string REMOVE_BID = "sp_RemoveBid";
        //Paymentinfo 
        public const string ADD_UPDATE_PAYMENTINFO = "sp_AddUpdatePaymentInfo";
        public const string GET_PAYMENTINFO_BY_ORDERID = "SP_GetPaymentInfoByOrderId";
        public const string GET_PAYMENTINFO_BY_CUSTOMERID = "SP_GetPaymentInfoByCustomerId";

        // DASHBOARDS
        public const string DASHBOARD_GET_BY_SELLER = "sp_GetSellerDashboard";
        public const string DASHBOARD_GET_BY_BUYER = "sp_GetBuyerDashboard";
        public const string DASHBOARD_GET_BY_ADMIN = "sp_GetAdminDashboard";
    }
}
