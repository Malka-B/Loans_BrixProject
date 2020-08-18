using Rules.Service.Models;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface IRuleService
    {                
        Task UpdatePolicyRules(RegisterModel registerModel);
        Task RegisterToProvideLoans(RegisterModel registerModel);
    }
}
