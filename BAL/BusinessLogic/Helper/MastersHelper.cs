using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using DAL;
using DAL.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class MastersHelper : IMastersHelper
    {
        private readonly IsqlDataHelper _sqlDataHelper;
        public MastersHelper(IsqlDataHelper sqlDataHelper) { 
            _sqlDataHelper = sqlDataHelper;
        }

        public async Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC)
        {
            var response = new Response<NDCUPC>();

            try
            {
                MySqlCommand cmdProduct = new MySqlCommand(StoredProcedures.MASTERS_GET_NDCUPC_DETAILS);
                cmdProduct.CommandType = CommandType.StoredProcedure;
                cmdProduct.Parameters.AddWithValue("@p_NDC", NDC);
                cmdProduct.Parameters.AddWithValue("@p_UPC", UPC);

                DataTable tblNDCUPC = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(cmdProduct));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToNDCUPCList(tblNDCUPC);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        public async Task<Response<OrderStatus>> GetOrderStatuses()
        {
            var response = new Response<OrderStatus>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_GET_ORDER_STATUSES);
                command.CommandType = CommandType.StoredProcedure;

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToOrderStatusList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        public async Task<Response<ProductCategory>> GetProductCategories(int categoryId = 0)
        {
            var response = new Response<ProductCategory>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_GET_PRODUCT_CATEGORIES);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_ProductCategoryId", categoryId);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToProductCategoryList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        public async Task<Response<ProductCategory>> AddProductCategory(ProductCategory productCategory)
        {
            var response = new Response<ProductCategory>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_ADD_UPDATE_PRODUCT_CATEGORY);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_ProductCategoryId", productCategory.ProductCategoryId);
                command.Parameters.AddWithValue("@p_CategoryName", productCategory.CategoryName);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Added Product Category.";
                response.Result = MapDataTableToProductCategoryList(tblCommandResult);
            }
            catch (Exception ex)
            {  
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        public async Task<Response<ProductCategory>> RemoveProductCategory(int categoryId)
        {
            var response = new Response<ProductCategory>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_REMOVE_PRODUCT_CATEGORY);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_ProductCategoryId", categoryId);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Removed Product Category.";
                response.Result = MapDataTableToProductCategoryList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }
        public async Task<Response<CategorySpecification>> GetCategoriesSpecification(int CategorySpecificationId = 0)
        {
            var response = new Response<CategorySpecification>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_GET_CATEGORY_SPECIFICATION);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_CategorySpecificationId", CategorySpecificationId);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToCategorySpecificationList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        public async Task<Response<CategorySpecification>> AddCategorySpecification(CategorySpecification categorySpecification)
        {
            var response = new Response<CategorySpecification>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_ADD_UPDATE_CATEGORY_SPECIFICATION);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_CategorySpecificationId", categorySpecification.CategorySpecificationId);
                command.Parameters.AddWithValue("@p_SpecificationName", categorySpecification.SpecificationName);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Added Product Category.";
                response.Result = MapDataTableToCategorySpecificationList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }
        public async Task<Response<CategorySpecification>> RemoveCategorySpecification(int CategorySpecificationId)
        {
            var response = new Response<CategorySpecification>();

            try
            {
                MySqlCommand command = new MySqlCommand(StoredProcedures.MASTERS_REMOVE_CATEGORY_SPECIFICATION);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_CategorySpecificationId", CategorySpecificationId);

                DataTable tblCommandResult = await Task.Run(() => _sqlDataHelper.ExecuteDataTableAsync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Removed Product Category.";
                response.Result = MapDataTableToCategorySpecificationList(tblCommandResult);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }

            return response;
        }

        #region Mapping Methods
        private static List<NDCUPC> MapDataTableToNDCUPCList(DataTable tblNDCUPC)
        {
            List<NDCUPC> lstNDCUPC = new List<NDCUPC>();
            foreach (DataRow ndcupc in tblNDCUPC.Rows)
            {
                NDCUPC item = new NDCUPC();
                item.Id = Convert.ToInt32(ndcupc["Id"]);
                item.NDC = ndcupc["NDC"].ToString() ?? "";
                item.UPC = ndcupc["UPC"].ToString() ?? "";
                item.ProductName = ndcupc["ProductName"].ToString() ?? "";
                item.ManufacturerName = ndcupc["ManufacturerName"].ToString() ?? "";
                item.Size = ndcupc["Size"].ToString() ?? "";
                item.UnitOfMeasurement = ndcupc["UnitOfMeasurement"].ToString() ?? "";
                item.Form = ndcupc["Form"].ToString() ?? "";

                lstNDCUPC.Add(item);
            }
            return lstNDCUPC;
        }

        private static List<ProductCategory> MapDataTableToProductCategoryList(DataTable tblProductCategory)
        {
            List<ProductCategory> lstProductCategory = new List<ProductCategory>();
            foreach (DataRow category in tblProductCategory.Rows)
            {
                ProductCategory item = new ProductCategory();
                item.ProductCategoryId = Convert.ToInt32(category["ProductCategoryId"]);                
                item.CategoryName = category["CategoryName"].ToString() ?? "";

                lstProductCategory.Add(item);
            }
            return lstProductCategory;
        }
        private static List<OrderStatus> MapDataTableToOrderStatusList(DataTable tblOrderStatus)
        {
            List<OrderStatus> lstOrderStatus = new List<OrderStatus>();
            foreach (DataRow oStatus in tblOrderStatus.Rows)
            {
                OrderStatus item = new OrderStatus();
                item.StatusId = Convert.ToInt32(oStatus["StatusId"]);
                item.StatusDescription = oStatus["StatusDescription"].ToString() ?? "";

                lstOrderStatus.Add(item);
            }
            return lstOrderStatus;
        }
        private static List<CategorySpecification> MapDataTableToCategorySpecificationList(DataTable tblcategoryspecification)
        {
            List<CategorySpecification> lstCategoryspecification = new List<CategorySpecification>();
            foreach (DataRow category in tblcategoryspecification.Rows)
            {
                CategorySpecification item = new CategorySpecification();
                item.CategorySpecificationId = Convert.ToInt32(category["CategorySpecificationId"]);
                item.SpecificationName = category["SpecificationName"].ToString() ?? "";

                lstCategoryspecification.Add(item);
            }
            return lstCategoryspecification;
        }
        #endregion Mapping Methonds
    }
}
