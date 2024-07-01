using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public interface IsqlDataHelper
    {
        Task<int> ExcuteNonQueryasync(SqlCommand cmd);
        Task<DataTable> SqlDataAdapterasync(SqlCommand cmd);
    }
}
