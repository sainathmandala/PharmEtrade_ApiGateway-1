﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class UserEmailResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<UserEmailViewModel> userlist { get; set; }
    }
}
