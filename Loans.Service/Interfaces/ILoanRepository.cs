using Enums;
using Loans.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface ILoanRepository
    {
        Task<Guid> Create(LoanModel loanModel);
        Task<Guid> Update(Guid loanId,LoanModel loanModel);
        Task UpdateLoanStatus(Guid loanId, LoanStatus loanStatus);
        Task<bool> CheckLoanExist(Guid loanId);
        Task UpdateLoanFailureRules(Guid loanId, List<LoanFailureRuleModel> failureRules);
    }
}
