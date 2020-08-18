using System;

namespace Loans.Data.Entities
{
    public class LoanFailureRulesEntity
    {
        public int ID { get; set; }
        public Guid LoanId { get; set; }
        public int RuleId { get; set; }
        public string RuleDescription { get; set; }
        public string ManagerComment { get; set; }
        public string ManagerSignature { get; set; }
        public LoanEntity Loan { get; set; }
    }
}
