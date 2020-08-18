using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rules.Service.Interfaces
{
    public interface ICheckRuleRepository
    {
        Task<List<RuleModel>> getRules(Guid loanProviderId);
        List<RuleParameterModel> GetRulesParameterTypesDict();
    }
}
