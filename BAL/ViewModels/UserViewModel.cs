using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string lastname { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public int UsertypeId { get; set; }

        [Required]
        public string Accounttype { get; set; }

        public bool UpnMember { get; set; }


    }
}
