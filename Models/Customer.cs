using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LackLusterVideo.Models
{
    public class Customer
    {
            public int CustomerId { get; set; }

            public string Name_First { get; set; }

            public string Name_Middle_In { get; set; }

            public string Name_Last { get; set; }

            public string Add_Line1 { get; set; }

            public string Add_Line2 { get; set; }

            public string Add_City { get; set; }

            public string Add_State { get; set; }

            public int Add_Zip { get; set; }

            public long PhoneNumber { get; set; }

            public string Email { get; set; }

            public bool Newsletter { get; set; }

            public int AccountBalance { get; set; }

            public bool Active { get; set; }
    }
}
