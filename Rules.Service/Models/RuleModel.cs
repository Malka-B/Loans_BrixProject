using System;
using System.Collections.Generic;

namespace Rules.Service.Models
{
    public class RuleModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public RuleModel ParentRule { get; set; }
        public Guid LoanProviderId { get; set; }       
        public string Parameter { get; set; }
        public string Operator { get; set; }
        public string ValueToCompeare { get; set; }
        public List<RuleModel> ChildrenRules { get; set; }
    }
}
