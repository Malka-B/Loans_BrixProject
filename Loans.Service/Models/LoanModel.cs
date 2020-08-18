using Enums;
using System;

namespace Loans.Service.Models
{
    public class LoanModel
    {
        public Guid LoanProviderID { get; set; }
        public int BorrowerID { get; set; }        
        public int Salary { get; set; }
        public bool FixedSalary { get; set; }
        public DateTime BirthDate { get; set; }
        public int LoanAmount { get; set; }
        public int SeniorityYearsInBank { get; set; }
        public int NumberOfRepayments { get; set; }        
        public LoanPurpose LoanPurpose { get; set; }
        public LoanStatus LoanStatus { get; set; }

    }
}
