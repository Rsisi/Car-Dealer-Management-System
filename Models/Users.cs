using System;
using System.ComponentModel.DataAnnotations;

namespace team011.Models
{
    public class Users
    {
       
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public string user_role { get; set; }

    }
    
}

