using System;
using System.Collections.Generic;

namespace Loans.Service.Models
{
    public class ApproveLoanModel
    {
        public Guid LoanId { get; set; }
        public List<ApproveRuleModel> approveRules { get; set; }
    }
}
