using Enums;
using System;
using System.Collections.Generic;

namespace Loans.Data.Entities
{
    public class LoanEntity
    {
        public Guid ID { get; set; }        
        public Guid LoanProviderID { get; set; }
        public DateTime LoanDate { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public int BorrowerID { get; set; }
        public int Salary { get; set; }
        public bool FixedSalary { get; set; }
        public DateTime BirthDate { get; set; }        
        public int LoanAmount { get; set; }
        public int SeniorityYearsInBank { get; set; }
        public int NumberOfRepayments { get; set; }
        public LoanPurpose LoanPurpose { get; set; }
        public List<LoanFailureRulesEntity> FailureRules { get; set; }
        
    }
}
