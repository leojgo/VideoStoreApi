namespace VideoStoreApi.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string NameFirst { get; set; }
        public string NameMiddleIn { get; set; }
        public string NameLast { get; set; }
        public string AddLine1 { get; set; }
        public string AddLine2 { get; set; }
        public string AddCity { get; set; }
        public string AddState { get; set; }
        public int AddZip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Newsletter { get; set; }
        public int AccountBalance { get; set; }
        public bool Active { get; set; }
    }

    public class CustomerInfo
    {
        public int CustomerId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string PhoneNumber { get; set; }
        public long TransactionCount { get; set; }
    }
}
