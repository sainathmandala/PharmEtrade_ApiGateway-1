using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class LoginViewModel
    {
        public int statusCode { get; set; }
        public string Message { get; set; }
        public string LoginStatus { get; set; }      
        public string UserId { get; set; }       
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserEmail { get; set; }      
        public string UserType { get; set; }        
        public string Token { get; set; }
        public bool IsActive { get; set; }
    }
}
