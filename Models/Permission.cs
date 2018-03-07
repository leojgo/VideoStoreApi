using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoStoreApi.Models
{
    public class Permission
    {
        [Key]
        public int EMP_Permiss{get; set;}
        public bool EMP_Create{get; set;}
        public bool EMP_Edit{get; set;}
        public bool EMP_Disable{get; set;}
        public bool CUST_Create{get; set;}
        public bool CUST_Edit{get; set;}
        public bool CUST_Disable{get; set;}
        public bool CUST_Search{get; set;}
        public bool CUST_ViewHist{get; set;}
        public bool Cust_EditHist{get; set;}
        public bool INV_Add{get; set;}
        public bool INV_Edit{get; set;}
        public bool INV_Disable{get; set;}
        public bool INV_Rent{get; set;}
        public bool INV_Return{get; set;}
        public bool INV_Hold{get; set;}
        public bool REP_Overdue{get; set;}
        public bool REP_Popular{get; set;}
        public bool REP_CheckedOut{get; set;}
        public bool REP_OnHold { get; set; }
    }

}
