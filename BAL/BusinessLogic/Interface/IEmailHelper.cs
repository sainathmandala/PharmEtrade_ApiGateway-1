using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IEmailHelper
    {
        Task SendEmail(string toMailAddress, string ccMailAddress, string mailSubject, string mailBody);
        Task SendEmail(string toMailAddress, string ccMailAddress, string mailSubject, string mailBody, MemoryStream attachementStream);
    }
}
