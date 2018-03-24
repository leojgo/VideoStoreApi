namespace VideoStoreApi.Models
{
    public class Permission
    {
        public int EmpPermiss { get; set; }
        public bool EmpCreate {get; set;}
        public bool EmpEdit {get; set;}
        public bool EmpDisable {get; set;}
        public bool CustCreate {get; set;}
        public bool CustEdit {get; set;}
        public bool CustDisable {get; set;}
        public bool CustSearch {get; set;}
        public bool CustViewHist {get; set;}
        public bool CustEditHist {get; set;}
        public bool InvAdd {get; set;}
        public bool InvEdit {get; set;}
        public bool InvDisable {get; set;}
        public bool InvRent {get; set;}
        public bool InvReturn {get; set;}
        public bool InvHold {get; set;}
        public bool RepOverdue {get; set;}
        public bool RepPopular {get; set;}
        public bool RepCheckedOut {get; set;}
        public bool RepOnHold {get; set;}

    }
}
