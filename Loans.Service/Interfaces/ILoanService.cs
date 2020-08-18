using Loans.Service.Models;
using Messages.Events;
using System;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface ILoanService
    {
        Task<Guid> Create(LoanModel loanModel);
        Task<Guid> Update(Guid loanId, LoanModel loanModel);
        Task UpdateLoanAfterCheckLegality(LoanChecked message);
    }
}
