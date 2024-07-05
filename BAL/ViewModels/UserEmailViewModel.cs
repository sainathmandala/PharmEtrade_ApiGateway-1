using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class UserEmailViewModel
    {
        public int UserId { get; set; }     
        public string Username { get; set; }       
        public string Email { get; set; }      
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
