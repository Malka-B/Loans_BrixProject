using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface IRuleRepository
    {       
       Task CreatePolicy(List<RuleModel> policyRules);
       Task<bool> CheckProviderLoanExist(Guid LoanProviderId);
       Task UpdatePolicy(Guid providerLoanId, List<RuleModel> policyRules);
    }
}
