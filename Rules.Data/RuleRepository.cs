using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rules.Data.Entities;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Rules.Data
{
    public class RuleRepository : IRuleRepository
    {
        private readonly RuleContext _rulesContext;
        private readonly IMapper _mapper;

        public RuleRepository(RuleContext rulesContext, IMapper mapper)
        {
            _rulesContext = rulesContext;
            _mapper = mapper;
        }

        public async Task<bool> CheckProviderLoanExist(Guid LoanProviderId)
        {
            Rule rule = await _rulesContext.Rule
                .FirstOrDefaultAsync(r => r.LoanProviderId == LoanProviderId);
            if (rule != null)
            {
                return true;
            }
            return false;
        }

        public async Task CreatePolicy(List<RuleModel> policyRules)
        {
            List<Rule> policyRulesList = _mapper.Map<List<Rule>>(policyRules);            
            await _rulesContext.AddRangeAsync(policyRulesList);
            await _rulesContext.SaveChangesAsync(); 
        }

        public async Task UpdatePolicy(Guid providerLoanId, List<RuleModel> policyRules)
        {
            List<Rule> policyRulesList = _mapper.Map<List<Rule>>(policyRules);
            List<Rule> policyRulesListToUpdate = await _rulesContext.Rule.Where(r => r.LoanProviderId == providerLoanId).ToListAsync();
            _rulesContext.RemoveRange(policyRulesListToUpdate);
            await _rulesContext.AddRangeAsync(policyRulesList);
            await _rulesContext.SaveChangesAsync();
        }
    }
}
