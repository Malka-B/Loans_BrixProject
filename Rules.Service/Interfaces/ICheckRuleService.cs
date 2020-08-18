using Messages.Commands;
using Messages.Events;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface ICheckRuleService
    {
        Task<LoanChecked> CheckLegality(CheckLoanValid loanDetails);
    }
}
