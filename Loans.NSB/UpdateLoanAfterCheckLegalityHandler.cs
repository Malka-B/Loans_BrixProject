using Loans.Service.Interfaces;
using Messages.Events;
using Messages.MessagesOjects;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loans.NSB
{
    public class UpdateLoanAfterCheckLegalityHandler : IHandleMessages<LoanChecked>
    {
        private readonly ILoanService _loansService;
        static ILog log = LogManager.GetLogger<UpdateLoanAfterCheckLegalityHandler>();

        public UpdateLoanAfterCheckLegalityHandler(ILoanService loansService)
        {
            _loansService = loansService;
        }

        public async Task Handle(LoanChecked message, IMessageHandlerContext context)
        {
            log.Info($"in UpdateLoanAfterCheckLegalityHandler loanId : {message.LoanId}");
            PrintTreeForEachRoot(message.IsLoanValid, message.CheckedRuleTree);
            await _loansService.UpdateLoanAfterCheckLegality(message);
        }

        //להוציא למחלקה
        private void PrintTreeForEachRoot(bool isLoanValid, List<RuleTreeNode> tree)
        {
            if (isLoanValid)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The loan is valid: Decision Tree:");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The loan is invalid: Decision Tree");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            List<RuleTreeNode> ruleTreeNodeRoots = tree.Where(rl => rl.ParentRuleId == 0).ToList();
            foreach (var root in ruleTreeNodeRoots)
            {
                PrintTree(root);
            }
        }
        private static void PrintTree(RuleTreeNode tree)
        {
            List<RuleTreeNode> firstStack = new List<RuleTreeNode>();
            firstStack.Add(tree);

            List<List<RuleTreeNode>> childListStack = new List<List<RuleTreeNode>>();
            childListStack.Add(firstStack);

            while (childListStack.Count > 0)
            {
                List<RuleTreeNode> childStack = childListStack[childListStack.Count - 1];

                if (childStack.Count == 0)
                {
                    childListStack.RemoveAt(childListStack.Count - 1);
                }
                else
                {
                    tree = childStack[0];
                    childStack.RemoveAt(0);

                    string indent = "";
                    for (int i = 0; i < childListStack.Count - 1; i++)
                    {
                        indent += (childListStack[i].Count > 0) ? "|  " : "   ";
                    }

                    if (tree.IsRuleValid)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(indent + "+- " + tree.RuleDescription);
                    }
                    else if (!tree.IsRuleValid)
                    {
                        if (tree.RuleDescription == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine(indent + "+- " + tree.RuleId);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(indent + "+- " + tree.RuleDescription);
                        }
                    }                    
                    if (tree.ChildrenRules.Count > 0)
                    {
                        childListStack.Add(new List<RuleTreeNode>(tree.ChildrenRules));
                    }
                }
            }
        }
    }
}
