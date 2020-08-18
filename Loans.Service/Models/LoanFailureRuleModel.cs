using System;

namespace Loans.Service.Models
{
    public class LoanFailureRuleModel
    {
        public Guid LoanId { get; set; }
        public int RuleId { get; set; }
        public string RuleDescription { get; set; }
        public string ManagerComment { get; set; }
        public string ManagerSignature { get; set; }
    }
}
