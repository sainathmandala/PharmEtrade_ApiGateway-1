using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public interface IsqlDataHelper
    {
        Task<int> ExcuteNonQueryasync(MySqlCommand cmd);
        Task<DataTable> SqlDataAdapterasync(MySqlCommand cmd);
        Task<MySqlDataReader> ExecuteReaderAsync(MySqlCommand cmd);
    }
}
