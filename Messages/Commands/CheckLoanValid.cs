using Messages.MessagesOjects;
using System.Collections.Generic;

namespace Messages.Commands
{
    public class CheckLoanValid
    {
        public LoanDetails LoanDetails { get; set; }
        public List<int> IgnoreRules { get; set; }//sec obj    
    }
}
