using System.Collections.Generic;

namespace Messages.MessagesOjects
{
    public class RuleTreeNode
    {
        public int ParentRuleId { get; set; }//ParentRule.id
        public int RuleId { get; set; }//id
        public string RuleDescription { get; set; }//
        public bool IsRuleValid { get; set; }//
        public List<RuleTreeNode> ChildrenRules { get; set; }
    }
}
