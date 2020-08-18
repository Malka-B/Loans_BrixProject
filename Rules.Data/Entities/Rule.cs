using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rules.Data.Entities
{
    public class Rule
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public Rule ParentRule { get; set; }
        public Guid LoanProviderId { get; set; }        
        public string Parameter { get; set; }
        public string Operator { get; set; }
        public string ValueToCompeare { get; set; }          
        public List<Rule> ChildrenRules { get; set; }
    }
}
