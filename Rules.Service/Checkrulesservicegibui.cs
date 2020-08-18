//using AutoMapper;
//using Exceptions;
//using Messages.Commands;
//using Messages.Events;
//using Messages.MessagesOjects;
//using Rules.Service.Extensions;
//using Rules.Service.Interfaces;
//using Rules.Service.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Rules.Service
//{
//    public class CheckRulesService : ICheckRulesService
//    {
//        private readonly ICheckRulesRepository _checkRulesRepository;
//        private readonly IMapper _mapper;
//        //private readonly Dictionary<string, string> RulesParameterTypes = new Dictionary<string, string>() {
//        //    { "Salary", "int" },
//        //    { "FixedSalary", "bool"},
//        //    {"Age", "int" },
//        //    {"LoanAmount", "int" },
//        //    {"SeniorityYearsInBank", "int" },
//        //    {"NumberOfRepayments", "int" },
//        //    {"LoanPurpose", "string" }
//        //};
//        private Dictionary<string, string> RulesParameterTypesDict = new Dictionary<string, string>();
//        public CheckRulesService(ICheckRulesRepository checkRulesRepository, IMapper mapper)
//        {
//            _checkRulesRepository = checkRulesRepository;
//            _mapper = mapper;
//            SetRulesParameterTypesDict();
//        }

//        private void SetRulesParameterTypesDict()
//        {
//            List<RulesParameterModel> dictionary = _checkRulesRepository.GetRulesParameterTypesDict();
//            foreach (var d in dictionary)
//            {
//                RulesParameterTypesDict.Add(d.ParameterName, d.ParameterType);
//            }
//        }

//        public async Task<LoanChecked> CheckLegality(CheckLoanValid message)
//        {
//            List<RuleModel> rulesTree = await _checkRulesRepository.getRules(message.LoanProviderId);
//            if (rulesTree.Count() == 0)
//            {
//                throw new LoanProviderNotFoundException();
//                //write to db log
//            }
//            LoanChecked loanChecked = new LoanChecked();
//            ScanTreeToCheckIsLoanVaild(rulesTree, message, loanChecked);
//            return loanChecked;
//        }
//        //scanProviderTreeRules
//        //לחלק את CheckLoanValid message
//        private void ScanTreeToCheckIsLoanVaild(List<RuleModel> rulesTree, CheckLoanValid message, LoanChecked loanChecked)
//        {
//            bool isBranchValid = true;
//            // rulesCheckedTree that contains what scanned Nodes in oreder to represent it to client
//            List<RuleTreeNode> ruleCheckedTree = _mapper.Map<List<RuleTreeNode>>(rulesTree);
//            List<RuleModel> treeRoots = rulesTree.Where(rl => rl.ParentRule == null).ToList();
//            List<RuleTreeNode> ruleTreeNodeRoots = ruleCheckedTree.Where(rl => rl.ParentRuleId == 0).ToList();
//            bool isWholeBranchTrue = false;
//            for (int i = 0; i < treeRoots.Count; i++)
//            {
//                //checkbranch
//                Stack<RuleTreeNode> ruleTreeNodeStack = new Stack<RuleTreeNode>();
//                Stack<RuleModel> rulesStack = new Stack<RuleModel>();
//                ruleTreeNodeStack.Push(ruleTreeNodeRoots[i]);
//                rulesStack.Push(treeRoots[i]);
//                //check node
//                while (!(rulesStack.Count == 0))
//                {
//                    var ruleTreeStackItem = rulesStack.Pop();
//                    var ruleTreeNodeStackItem = ruleTreeNodeStack.Pop();
//                    for (int j = 0; j < ruleTreeStackItem.ChildrenRules.Count; j++)
//                    {
//                        rulesStack.Push(ruleTreeStackItem.ChildrenRules[j]);
//                        ruleTreeNodeStack.Push(ruleTreeNodeStackItem.ChildrenRules[j]);
//                    }
//                    var loanValue = message.GetType().GetProperty(ruleTreeStackItem.Parameter).GetValue(message, null);
//                    bool isRuleValid =////get params as obj
//                        IsRuleValid(loanValue, ruleTreeStackItem.Parameter, ruleTreeStackItem.Operator, ruleTreeStackItem.ValueToCompeare);
//                    //CheckIsRuleValidValue(isRuleValid, message, ruleTreeNodeStackItem, ruleTreeStackItem, val, loanChecked, ref isBranchValid, ruleCheckedTree);//לבדוק שאכן loanChecked משתנה.
//                    //111
//                    if (isRuleValid || (message.IsLoanApproveByManager && message.IgnoreRules.Contains(ruleTreeStackItem.Id)))
//                    {
//                        SetValidRuleTreeNode(loanValue, ruleTreeNodeStackItem, ruleTreeStackItem);
//                        //get desc set here
//                        if (ruleTreeStackItem.ChildrenRules.Count == 0)
//                        {
//                            if (isBranchValid)
//                            {
//                                isWholeBranchTrue = true;
//                                break;
//                            }
//                            isBranchValid = true;
//                        }
//                    }
//                    else
//                    {
//                        SetInvalidRuleTreeNode(loanValue, ruleTreeNodeStackItem, ruleTreeStackItem, ref isBranchValid);
//                    }
//                }
//                if (isWholeBranchTrue)
//                {
//                    break;
//                }
//            }
//            if (isWholeBranchTrue)
//            {
//                SetValidLoanCheckedObjectAndReturn(loanChecked, message, ruleCheckedTree);
//            }
//            else
//            {
//                SetInvalidLoanCheckedObjectAndReturn(loanChecked, message, ruleCheckedTree);
//            }
//        }

//        private void SetInvalidLoanCheckedObjectAndReturn(LoanChecked loanChecked, CheckLoanValid message, List<RuleTreeNode> ruleCheckedTree)
//        {
//            loanChecked.IsLoanValid = false;
//            loanChecked.CheckedRuleTree = ruleCheckedTree;
//            loanChecked.LoanId = message.LoanId;
//            loanChecked.IsLoanApproveByManager = message.IsLoanApproveByManager;
//            return;
//        }

//        //private void CheckIsRuleValidValue(bool isRuleValid, CheckLoanValid message, RuleTreeNode ruleTreeNodeStackItem, RuleModel ruleTreeStackItem, object val, LoanChecked loanChecked, ref bool isBranchValid, List<RuleTreeNode> ruleCheckedTree)
//        //{
//        //    if (isRuleValid || (message.IsLoanApproveByManager && message.IgnoreRules.Contains(ruleTreeStackItem.Id)))
//        //    {
//        //        SetValidRuleTreeNode(message, ruleTreeNodeStackItem, ruleTreeStackItem, loanChecked, ref isBranchValid, ruleCheckedTree);
//        //    }
//        //    else
//        //    {
//        //        SetInvalidRuleTreeNode(val, ruleTreeNodeStackItem, ruleTreeStackItem, ref isBranchValid);
//        //    }
//        //}

//        //get description ret string
//        private void SetValidRuleTreeNode(object loanVal, RuleTreeNode ruleTreeNodeStackItem, RuleModel ruleTreeStackItem)
//        {
//            string ruleDescription =
//               $"Valid rule: {ruleTreeStackItem.Parameter} = {loanVal} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
//            ruleTreeNodeStackItem.RuleDescription = ruleDescription;
//            ruleTreeNodeStackItem.IsRuleValid = true;
//        }

//        private void SetValidLoanCheckedObjectAndReturn(LoanChecked loanChecked, CheckLoanValid message, List<RuleTreeNode> ruleCheckedTree)
//        {
//            loanChecked.IsLoanValid = true;
//            loanChecked.CheckedRuleTree = ruleCheckedTree;
//            loanChecked.LoanId = message.LoanId;
//            loanChecked.IsLoanApproveByManager = message.IsLoanApproveByManager;
//            return;
//        }

//        private void SetInvalidRuleTreeNode(object val, RuleTreeNode ruleTreeNodeStackItem, RuleModel ruleTreeStackItem, ref bool isBranchValid)
//        {
//            isBranchValid = false;
//            string ruleDescription =
//                $"Invalid: {ruleTreeStackItem.Parameter} = {val} for rule: {ruleTreeStackItem.Parameter} {ruleTreeStackItem.Operator} {ruleTreeStackItem.ValueToCompeare}.";
//            ruleTreeNodeStackItem.RuleDescription = ruleDescription;
//            ruleTreeNodeStackItem.IsRuleValid = false;
//            if (ruleTreeStackItem.ChildrenRules.Count == 0)
//            {
//                isBranchValid = true;
//            }
//        }

//        private Expression<Func<T, bool>> GetRuleExpression<T>(string parameter, string operator1, T valToCompare)
//        {
//            ParameterExpression param = Expression.Parameter(typeof(T), parameter);//שם פרמטר הלמדה וסוג התכולה שלו            
//            ConstantExpression valueToCompare = Expression.Constant(valToCompare);//הערך אליו משווים והסוג שלו 

//            BinaryExpression parameterOperatorValToCompare = operator1.Operator(param, valueToCompare);// LessThan אופרטור ההשוואה
//            Expression<Func<T, bool>> ruleExpression =
//                Expression.Lambda<Func<T, bool>>(
//                    parameterOperatorValToCompare,
//                    new ParameterExpression[] { param });
//            return ruleExpression;
//        }
//        //get params as obj
//        private bool IsRuleValid(object valTocheck, string parameter, string operator1, object valToCompare)
//        {
//            var parameterType = RulesParameterTypesDict[parameter];
//            switch (parameterType)
//            {
//                case "int":
//                    Expression<Func<int, bool>> intRuleExpression = GetRuleExpression(parameter, operator1, Convert.ToInt32(valToCompare));
//                    return intRuleExpression.Compile()((int)valTocheck);
//                case "bool":
//                    Expression<Func<bool, bool>> boolRuleExpression = GetRuleExpression(parameter, operator1, Convert.ToBoolean(valToCompare));
//                    return boolRuleExpression.Compile()((bool)valTocheck);
//                case "string":
//                    Expression<Func<string, bool>> stringRuleExpression = GetRuleExpression(parameter, operator1, Convert.ToString(valToCompare));
//                    return stringRuleExpression.Compile()((string)valTocheck);
//                default:
//                    return false;
//            }
//        }

//    }
//}
