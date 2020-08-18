using Messages.MessagesOjects;
using System;
using System.Collections.Generic;

namespace Messages.Events
{
    public class LoanChecked
    {
        public Guid LoanId { get; set; }//in main func       
        public bool IsLoanValid { get; set; }        
        public List<RuleTreeNode> CheckedRuleTree { get; set; }
    }
}
