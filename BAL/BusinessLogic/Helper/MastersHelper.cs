using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using DAL;
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
                MySqlCommand cmdProduct = new MySqlCommand(StoredProcedures.GET_NDCUPC_DETAILS);
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
    }
}
