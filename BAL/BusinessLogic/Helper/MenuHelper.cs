using BAL.BusinessLogic.Interface;
using BAL.Models;
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
    public class MenuHelper : IMenuHelper
    {
        private IsqlDataHelper _sqlDataHelper;
        public MenuHelper(IsqlDataHelper isqlDataHelper) { 
            _sqlDataHelper = isqlDataHelper;
        }
        public async Task<Response<Menu>> GetMenuByAccountType(int accountTypeId = 0)
        {
            var response = new Response<Menu>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_GetMenuByAccountType");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_AccountTypeId", accountTypeId);
                DataTable tblMenu = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));                
                List<Menu> lstMenu = new List<Menu>();
                foreach (DataRow menu in tblMenu.Rows)
                {
                    Menu menuItem = new Menu();
                    menuItem.MenuId = Convert.ToInt32(menu["MenuId"]);
                    menuItem.MenuName = menu["MenuName"].ToString();
                    menuItem.NavigateUrl = menu["NavigateUrl"].ToString();
                    menuItem.Parent = Convert.ToInt32(menu["Parent"]);
                    menuItem.Description = menu["Description"].ToString();
                    menuItem.IsActive = Convert.ToInt32(menu["IsActive"]) == 0 ? true : false;
                    menuItem.AccountTypeId = Convert.ToInt32(menu["AccountTypeId"]);
                    menuItem.CreatedOn = menu["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(menu["CreatedOn"]) : DateTime.MinValue;
                    menuItem.ModifiedOn = menu["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(menu["ModifiedOn"]) : DateTime.MinValue;
                    lstMenu.Add(menuItem);
                }
                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = lstMenu;
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }
    }
}
