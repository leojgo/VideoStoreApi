using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoStoreApi.Models
{
    public class Login
    {
        public int username { get; set; }
        public string password { get; set; }
    }

    public class Logout
    {
        public int username { get; set; }
    }
}
