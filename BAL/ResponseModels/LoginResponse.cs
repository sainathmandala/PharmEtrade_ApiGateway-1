using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class LoginResponse
    {
        public int StatusCode{ get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public int UserTypeId { get; set; }
        public string Token { get; set; }
    }
}
