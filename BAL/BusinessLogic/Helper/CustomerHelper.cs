using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModels;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class CustomerHelper: IcustomerHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;

        public CustomerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
        }

        // Author: [Shiva]
        // Created Date: [29/06/2024]
        // Description: Method for Customer login
        public async Task<DataTable> CustomerLogin(string username, string password)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("SP_CustomerLogin", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", username);
                cmd.Parameters.AddWithValue("@Password", password);
                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "CustomerLogin_SP :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        public async Task<int> AddToCart(int userId, int imageId, int productId)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("InsertAddtoCartProduct", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Userid", userId);
                cmd.Parameters.AddWithValue("@Imageid", imageId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                return await Task.Run(() => _isqlDataHelper.ExcuteNonQueryasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "AddToCart_SP :  errormessage:" + ex.Message.ToString()));
                throw ex;
            }
        }

        public Task<int> dummy(int userId, int imageId, int productId)
        {
            throw new NotImplementedException();
        }

        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for registration of User 
        public async Task<string> SaveCustomerData(UserViewModel userViewModel)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("SP_InsertUser", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userViewModel.Username);
                cmd.Parameters.AddWithValue("@Email", userViewModel.Email);
                cmd.Parameters.AddWithValue("@Password", userViewModel.Password);
                cmd.Parameters.AddWithValue("@PhoneNumber", userViewModel.PhoneNumber);
                cmd.Parameters.AddWithValue("@RoleId", userViewModel.RoleId);

                await sqlcon.OpenAsync();
                await _isqlDataHelper.ExcuteNonQueryasync(cmd);
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "SaveUser :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }


        // Author: [Shiva]
        // Created Date: [02/07/2024]
        // Description: Method for Get the data of  User based on UserId
        public async Task<DataTable> GetUserDetailsById(int userId)
        {

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_GetUserById", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", userId);


                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetusersdataById_sp :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        // Author: [Shiva]
        // Created Date: [03/07/2024]
        // Description: Method for update password
        public async Task<string> UpdatePassword(int id,  string newPassword)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_UpdatePassword", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
               

                await sqlcon.OpenAsync();
                string result = await cmd.ExecuteScalarAsync() as string;
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "UpdatePassword: ErrorMessage - " + ex.Message.ToString()));
                throw ex;
            }
        }


        // Author: [Shiva]
        // Created Date: [04/07/2024]
        // Description: Method for Get the data of  User based on Email
        public async Task<DataTable> GetUserDetailsByEmail(string email)
        {

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_GetUserByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);


                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetusersdataByEmail_sp :  errormessage:" + ex.Message.ToString()));

                throw ex;
            }
        }

        // Author: [Shiva]
        // Created Date: [08/07/2024]
        // Description: Method for update password by email(reset Password)
        public async Task<string> UpdatePasswordByEmail(string email, string newPassword)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("Sp_UpdatePasswordByEmail", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);


                await sqlcon.OpenAsync();
                string result = await cmd.ExecuteScalarAsync() as string;
                return "Success";
            }
            catch (Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "UpdatePasswordBymail: ErrorMessage - " + ex.Message.ToString()));
                throw ex;
            }
        }
    }
}
