using Loans.Service.Models;
using Messages.Commands;
using System.Threading.Tasks;

namespace Loans.Service.Interfaces
{
    public interface IManagerLoanService
    {        
        Task<CheckLoanValid> ApproveLoan(ApproveLoanModel approveLoanModel);
    }
}
