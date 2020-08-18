using System;
using System.Collections.Generic;
using System.Text;

namespace Rules.Service.Models
{
    public class RuleModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public RuleModel ParentRule { get; set; }
        public Guid LoanProviderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string Parameter { get; set; }
        public string Operator { get; set; }
        public string ValueToCompeare { get; set; }
        public List<RuleModel> ChildrenRules { get; set; }
    }
}
