using Enums;
using System;

namespace Messages.MessagesOjects
{
    public class LoanDetails
    {
        public Guid LoanId { get; set; }
        public Guid LoanProviderId { get; set; }
        public int Salary { get; set; }
        public bool FixedSalary { get; set; }
        public int Age { get; set; }
        public int LoanAmount { get; set; }
        public int SeniorityYearsInBank { get; set; }
        public int NumberOfRepayments { get; set; }
        public LoanPurpose LoanPurpose { get; set; }
    }
}
