using Loans.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface IManagerLoanRepository
    {
        Task<bool> CheckLoanExist(Guid loanId);
        Task<List<LoanFailureRuleModel>> GetFailureRulesByLoanId(Guid loanId);
        Task UpdateApproveFailureRules(Guid loanId, List<LoanFailureRuleModel> loanFailureRules);
        Task<LoanModel> GetLoanById(Guid loanId);
    }
}
