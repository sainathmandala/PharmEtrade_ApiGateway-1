using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class SqlDataHelper: IsqlDataHelper
    {
        private readonly string _connectionString;
        private string exFolder = Path.Combine("ExceptionLogs");
        private string exPathToSave = string.Empty;
        public SqlDataHelper(IConfiguration configuration)
        {
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
        }

        public async Task<int> ExcuteNonQueryasync(MySqlCommand cmd)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            int i = 0;
            try
            {
                await sqlcon.OpenAsync();
                cmd.Connection = sqlcon;
                i = await cmd.ExecuteNonQueryAsync();
                await sqlcon.CloseAsync();
                cmd.Dispose();
                return i;

            }
            catch (Exception ex)
            {
                sqlcon.Close();
                cmd.Dispose();
                throw ex;
            }
        }
        public async Task<DataTable> SqlDataAdapterasync(MySqlCommand cmd)
        {
            MySqlDataAdapter adp = new MySqlDataAdapter();
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            cmd.Connection = sqlcon;
            DataTable dt = new DataTable();
            try
            {
                await sqlcon.OpenAsync();
                adp = new MySqlDataAdapter(cmd);
                await Task.Run(() => adp.Fill(dt));
                await sqlcon.CloseAsync();
                cmd.Dispose();
                adp.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                sqlcon.Close();
                cmd.Dispose();
                adp.Dispose();
                throw ex;
            }
        }

        public async Task<DataTable> ExecuteDataTableAsync(MySqlCommand command)
        {            
            using (MySqlConnection dbConnection = new MySqlConnection(_connectionString))
            {
                command.Connection = dbConnection;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);                
                DataTable dt = new DataTable();
                try
                {
                    await dbConnection.OpenAsync();                    
                    await Task.Run(() => adapter.Fill(dt));
                    await dbConnection.CloseAsync();
                    command.Dispose();
                    adapter.Dispose();
                    return dt;
                }
                catch (Exception ex)
                {
                    dbConnection.Close();
                    command.Dispose();
                    adapter.Dispose();
                    return null;
                }
            }                
        }

        public async Task<MySqlDataReader> ExecuteReaderAsync(MySqlCommand cmd)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                cmd.Connection = sqlcon;
                MySqlDataReader reader;
                try
                {
                    await sqlcon.OpenAsync();
                    reader = cmd.ExecuteReader();
                    await sqlcon.CloseAsync();
                    cmd.Dispose();
                    return reader;
                }
                catch (Exception ex)
                {
                    sqlcon.Close();
                    cmd.Dispose();
                    return null;
                }
            }
        }
    }
}
