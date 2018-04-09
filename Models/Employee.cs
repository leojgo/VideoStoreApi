using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VideoStoreApi.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeType { get; set; }
        public string PwHash { get; set; }
        public bool Active { get; set; }
        public string EmployeeTitle { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TempEmployee : Employee
    {
        public string RawPw { get; set; }
    }

    public class EmployeeInfoToShare
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeType { get; set; }
        public bool Active { get; set; }
        public string EmployeeTitle { get; set; }
        public string PhoneNumber { get; set; }
    }
}

