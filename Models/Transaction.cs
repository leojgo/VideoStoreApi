using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VideoStoreApi.Models
{
    public class Transaction
    {
        public long TransId {get;set;}
        public string Date {get;set;}
        public int EmpId {get;set;}
        public int Fees {get;set;}
        public int FeesPaid {get;set;}
        public int TotalPaid {get;set;}
        public int RemBalance {get;set;}
        public int CustId {get;set;}
        public string PymtType { get; set; }
        public string PymtCard { get; set; }
    }
}
