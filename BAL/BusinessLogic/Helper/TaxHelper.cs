using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using DAL;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;
using BAL.ResponseModels;
using Microsoft.AspNetCore.Http;
using BAL.Models;
using BAL.RequestModels;
using System.Reflection;
using Org.BouncyCastle.Utilities.Net;
using Org.BouncyCastle.Asn1.X509;

namespace BAL.BusinessLogic.Helper
{
    public class TaxHelper : ITaxHelper
    {

        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;
        private readonly SmtpSettings _smtpSettings;
        private readonly S3Helper _s3Helper;
        private readonly IEmailHelper _emailHelper;
        private readonly IJwtHelper _jwtHelper;

        public TaxHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, SmtpSettings smtpSettings, IJwtHelper jwtHelper, IEmailHelper emailHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _smtpSettings = smtpSettings;
            _s3Helper = new S3Helper(configuration);
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
        }
        public async Task<Response<TaxInformation>> AddTaxInformationDetails(TaxInformation taxInformation)
        {
            Response<TaxInformation> response = new Response<TaxInformation>();
            try
            {
                MySqlCommand cmdTaxInformation = new MySqlCommand(StoredProcedures.ADD_TAXINFORMATIONDETAILS);
                cmdTaxInformation.CommandType = CommandType.StoredProcedure;
                cmdTaxInformation.Parameters.AddWithValue("@p_StateName", taxInformation.StateName);
                cmdTaxInformation.Parameters.AddWithValue("@p_CategorySpecificationID", taxInformation.CategorySpecificationID);
                cmdTaxInformation.Parameters.AddWithValue("@p_TaxPercentage", taxInformation.TaxPercentage);
                cmdTaxInformation.Parameters.AddWithValue("@p_IsActive", taxInformation.IsActive);
               

                DataTable tblTaxInformation = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdTaxInformation));

                response.StatusCode = 200;
                response.Message = "Beneficiary Added/Updated Successfully.";
                response.Result = MapDataTableToTaxInformationList(tblTaxInformation);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<TaxInformation>> GetAllTaxInformation()
        {
            Response<TaxInformation> response = new Response<TaxInformation>();
            try
            {
                MySqlCommand cmdTaxInformation = new MySqlCommand(StoredProcedures.TAXINFORMATION_GET_ALL);
                cmdTaxInformation .CommandType = CommandType.StoredProcedure;   

                DataTable tblTaxInformation = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdTaxInformation));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToTaxInformationList(tblTaxInformation);
            }
            catch (Exception ex)
            {
                // Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<TaxInformation>> GetTaxInformationByCategorySpecificationId(int categorySpecificationId)
        {
            Response<TaxInformation> response = new Response<TaxInformation>();
            try
            {
                MySqlCommand cmdTaxInformation = new MySqlCommand(StoredProcedures.TAXINFORMATION_GET_BY_CATEGORYSPECIFICATIONID);
                cmdTaxInformation.CommandType = CommandType.StoredProcedure;
                cmdTaxInformation.Parameters.AddWithValue("@p_CategorySpecificationID", categorySpecificationId);
               

                DataTable tblTaxInformation = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdTaxInformation));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToTaxInformationList(tblTaxInformation);
            }
            catch (Exception ex)
            {
                // Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<TaxInformation>> GetTaxInformationByStateName(string stateName)
        {
            Response<TaxInformation> response = new Response<TaxInformation>();
            try
            {
                MySqlCommand cmdTaxInformation = new MySqlCommand(StoredProcedures.TAXINFORMATION_GET_BY_STATENAME);
                cmdTaxInformation.CommandType = CommandType.StoredProcedure;
                cmdTaxInformation.Parameters.AddWithValue("@p_StateName", stateName);

                DataTable tblTaxInformation = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdTaxInformation));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToTaxInformationList(tblTaxInformation);
            }
            catch (Exception ex)
            {
                // Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<TaxInformation>> UpdateTaxInformation(TaxInformation taxInformation)
        {
            Response<TaxInformation> response = new Response<TaxInformation>();
            try
            {
                MySqlCommand cmdTaxInformation = new MySqlCommand(StoredProcedures.UPDATE_TAXINFORMATION_DETAILS);
                cmdTaxInformation.CommandType = CommandType.StoredProcedure;

                cmdTaxInformation.Parameters.AddWithValue("@p_StateName", taxInformation.StateName);
                cmdTaxInformation.Parameters.AddWithValue("@p_CategorySpecificationID", taxInformation.CategorySpecificationID);
                cmdTaxInformation.Parameters.AddWithValue("@p_IsActive", taxInformation.IsActive);
                cmdTaxInformation.Parameters.AddWithValue("@p_TaxPercentage", taxInformation.TaxPercentage);


                DataTable tblTaxiInformation = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdTaxInformation));

                response.StatusCode = 200;
                response.Message = "Address Added/Updated Successfully.";
                response.Result = MapDataTableToTaxInformationList(tblTaxiInformation);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        private List<TaxInformation> MapDataTableToTaxInformationList(DataTable tblBeneficiary)
        {
            List<TaxInformation> lstTaxInformation = new List<TaxInformation>();
            foreach (DataRow aItem in tblBeneficiary.Rows)
            {
                TaxInformation taxInformation = new TaxInformation();
                taxInformation.TaxInformationID = aItem["TaxInformationId"].ToString() ?? "";
                taxInformation.StateName = aItem["StateName"].ToString() ?? "";
                taxInformation.CategorySpecificationID = Convert.ToInt32(Convert.IsDBNull(aItem["CategorySpecificationID"]) ? 0 : aItem["CategorySpecificationID"]);
                taxInformation.TaxPercentage = Convert.ToInt32(Convert.IsDBNull(aItem["TaxPercentage"]) ? 0 : aItem["TaxPercentage"]);
                taxInformation.CreatedDate = aItem["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(aItem["CreatedDate"]) : (DateTime?)null;
                taxInformation.ModifiedDate = aItem["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(aItem["ModifiedDate"]) : (DateTime?)null;
                taxInformation.IsActive = Convert.ToInt32(Convert.IsDBNull(aItem["IsActive"]) ? 0 : aItem["IsActive"]);


                lstTaxInformation.Add(taxInformation);

                
            }
            return lstTaxInformation;
        }
    }
}
