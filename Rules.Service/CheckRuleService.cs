using AutoMapper;
using Exceptions;
using Messages.Commands;
using Messages.Events;
using Messages.MessagesOjects;
using Rules.Service.Extensions;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rules.Service
{
    public class CheckRuleService : ICheckRuleService
    {
        private readonly ICheckRuleRepository _checkRulesRepository;
        private readonly IMapper _mapper;
        private Dictionary<string, string> RulesParameterTypesDict = new Dictionary<string, string>();

        public CheckRuleService(ICheckRuleRepository checkRulesRepository, IMapper mapper)
        {
            _checkRulesRepository = checkRulesRepository;
            _mapper = mapper;
            SetRulesParameterTypes();//?
        }

        private void SetRulesParameterTypes()
        {
            List<RuleParameterModel> dictionary = _checkRulesRepository.GetRulesParameterTypesDict();
            foreach (var d in dictionary)
            {
                RulesParameterTypesDict.Add(d.ParameterName, d.ParameterType);
            }
        }

        public async Task<LoanChecked> CheckLegality(CheckLoanValid loanToCheck)
        {
            List<RuleModel> rulesTree = await _checkRulesRepository.getRules(loanToCheck.LoanDetails.LoanProviderId);
            if (rulesTree.Count() == 0)
            {
                throw new LoanProviderNotFoundException();
                //write to db log
            }
            LoanChecked loanChecked = ScanProviderTreeRules(rulesTree, loanToCheck);
            loanChecked.LoanId = loanToCheck.LoanDetails.LoanId;
            return loanChecked;
        }
        
        private LoanChecked ScanProviderTreeRules(List<RuleModel> rulesTree, CheckLoanValid loanToCheck)
        {
            LoanChecked loanChecked = new LoanChecked();            
            bool isBranchValid = true;            
            // rulesCheckedTree that contains what scanned in oreder to represent it to client
            List<RuleTreeNode> checkedRulesTree = _mapper.Map<List<RuleTreeNode>>(rulesTree);
            List<RuleModel> treeRoots = rulesTree.Where(rl => rl.ParentRule == null).ToList();
            List<RuleTreeNode> ruleTreeNodeRoots = checkedRulesTree.Where(rl => rl.ParentRuleId == 0).ToList();
            for (int i = 0; i < treeRoots.Count; i++)
            {
                isBranchValid = CheckBranchValid(treeRoots[i], ruleTreeNodeRoots[i], loanToCheck);
                if (isBranchValid)
                {
                    loanChecked = CreateValidCheckedLoan(checkedRulesTree);
                    return loanChecked;
                }
            }
            loanChecked = CreateInvalidCheckedLoan(checkedRulesTree);
            return loanChecked;
        }

        private LoanChecked CreateInvalidCheckedLoan(List<RuleTreeNode> checkedRulesTree)
        {
            LoanChecked checkedLoan = new LoanChecked
            {
                IsLoanValid = false,
                CheckedRuleTree = checkedRulesTree
            };

            return checkedLoan;
        }

        private LoanChecked CreateValidCheckedLoan(List<RuleTreeNode> checkedRulesTree)
        {
            LoanChecked checkedLoan = new LoanChecked
            {
                IsLoanValid = true,
                CheckedRuleTree = checkedRulesTree
            };
            return checkedLoan;
        }

        private bool CheckBranchValid(RuleModel treeRoot, RuleTreeNode treeNodeRoot, CheckLoanValid message)
        {
            //depth tree scan using stack
            bool isBranchValid = true; 
            Stack<RuleModel> treeStack = new Stack<RuleModel>();
            Stack<RuleTreeNode> treeNodeStack = new Stack<RuleTreeNode>();

            treeStack.Push(treeRoot);
            treeNodeStack.Push(treeNodeRoot);           
            while (!(treeStack.Count == 0))
            {
                var ruleTreeStackItem = treeStack.Pop();
                var ruleTreeNodeStackItem = treeNodeStack.Pop();

                for (int j = 0; j < ruleTreeStackItem.ChildrenRules.Count; j++)//create stack of branch
                {
                    treeStack.Push(ruleTreeStackItem.ChildrenRules[j]);
                    treeNodeStack.Push(ruleTreeNodeStackItem.ChildrenRules[j]);
                }

                var loanValueToCompare = message.LoanDetails.GetType().GetProperty(ruleTreeStackItem.Parameter).GetValue(message.LoanDetails, null);
                bool isRuleValid = CheckRuleValid(loanValueToCompare, ruleTreeStackItem, message.IgnoreRules);
                isBranchValid = isRuleValid ? isBranchValid : false;
                UpdateTreeNode(loanValueToCompare, ruleTreeNodeStackItem, ruleTreeStackItem, isRuleValid);                                                   //111

                if (ruleTreeStackItem.ChildrenRules.Count == 0)// at end of branch
                {
                    if (isBranchValid)
                    {
                        return true;
                    }
                    isBranchValid = true;
                }
            }
            return false;
        }

        private void UpdateTreeNode(object loanValueToCompare, RuleTreeNode ruleTreeNodeStackItem, RuleModel ruleTreeStackItem, bool isRuleValid)
        {
            if (isRuleValid)
            {
                ruleTreeNodeStackItem.RuleDescription = GetValidRuleDescription(loanValueToCompare, ruleTreeStackItem);
                ruleTreeNodeStackItem.IsRuleValid = true;
            }
            else
            {
                ruleTreeNodeStackItem.RuleDescription = GetInvalidRuleDescription(loanValueToCompare, ruleTreeStackItem);
                ruleTreeNodeStackItem.IsRuleValid = false;
            }
        }

        private string GetInvalidRuleDescription(object loanValueToCompare, RuleModel ruleTreeStackItem)
        {
            string ruleDescription =
                           $"Invalid: {ruleTreeStackItem.Parameter} = {loanValueToCompare} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
            return ruleDescription;
        }

        private bool CheckRuleValid(object loanValueToCompare, RuleModel ruleTreeStackItem, List<int> ignoreRules)
        {
            if (ignoreRules != null && ignoreRules.Contains(ruleTreeStackItem.Id))//approve by manager
            {
                return true;
            }
            var parameterType = RulesParameterTypesDict[ruleTreeStackItem.Parameter];
            switch (parameterType)
            {
                case "int":
                    Expression<Func<int, bool>> intRuleExpression = GetRuleExpression(ruleTreeStackItem.Parameter, ruleTreeStackItem.Operator, Convert.ToInt32(ruleTreeStackItem.ValueToCompeare));
                    return intRuleExpression.Compile()((int)loanValueToCompare);
                case "bool":
                    Expression<Func<bool, bool>> boolRuleExpression = GetRuleExpression(ruleTreeStackItem.Parameter, ruleTreeStackItem.Operator, Convert.ToBoolean(ruleTreeStackItem.ValueToCompeare));
                    return boolRuleExpression.Compile()((bool)loanValueToCompare);
                case "string":
                    Expression<Func<string, bool>> stringRuleExpression = GetRuleExpression(ruleTreeStackItem.Parameter, ruleTreeStackItem.Operator, Convert.ToString(ruleTreeStackItem.ValueToCompeare));
                    return stringRuleExpression.Compile()((string)loanValueToCompare);
                default:
                    return false;
            }
        }

        private string GetValidRuleDescription(object loanValue, RuleModel ruleTreeStackItem)
        {
            string ruleDescription =
               $"Valid rule: {ruleTreeStackItem.Parameter} = {loanValue} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
            return ruleDescription;
        }

        private Expression<Func<T, bool>> GetRuleExpression<T>(string parameter, string operator1, T valToCompare)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), parameter);//שם פרמטר הלמדה וסוג התכולה שלו            
            ConstantExpression valueToCompare = Expression.Constant(valToCompare);//הערך אליו משווים והסוג שלו 

            BinaryExpression parameterOperatorValToCompare = operator1.Operator(param, valueToCompare);// LessThan אופרטור ההשוואה
            Expression<Func<T, bool>> ruleExpression =
                Expression.Lambda<Func<T, bool>>(
                    parameterOperatorValToCompare,
                    new ParameterExpression[] { param });
            return ruleExpression;
        }
    }
}


//if (isRuleValid || (message.IsLoanApproveByManager && message.IgnoreRules.Contains(ruleTreeStackItem.Id)))//if rule valid
//{
//    string ruleDescription =
//        $"Valid rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
//    ruleTreeNodeStackItem.RuleDescription = ruleDescription;
//    ruleTreeNodeStackItem.IsRuleValid = true;
//    if (ruleTreeStackItem.ChildrenRules.Count == 0)
//    {
//        if (isBranchValid)
//        {
//            loanChecked.IsLoanValid = true;
//            loanChecked.CheckedRuleTree = ruleCheckedTree;
//            loanChecked.LoanId = message.LoanId;
//            loanChecked.IsLoanApproveByManager = message.IsLoanApproveByManager;
//            //return loanChecked;
//            return;
//        }
//        isBranchValid = true;
//    }
//}
//else if (!isRuleValid)//אם חוק לא היה תקין
// {
//isBranchValid = false;
//string ruleDescription =
//    $"Invalid: {ruleTreeStackItem.Parameter} = {val} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
//ruleTreeNodeStackItem.RuleDescription = ruleDescription;
//ruleTreeNodeStackItem.IsRuleValid = false;

//if (ruleTreeStackItem.ChildrenRules.Count == 0)
//{
//    isBranchValid = true;
//}
//  }