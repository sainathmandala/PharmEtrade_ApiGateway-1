using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IcustomerHelper
    {
        Task<DataTable> CustomerLogin(string username, string password);
    }
}
