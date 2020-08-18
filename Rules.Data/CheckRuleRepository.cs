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
    public class CheckRuleRepository : ICheckRuleRepository
    {
        private readonly RuleContext _rulesContext;
        private readonly IMapper _mapper;

        public CheckRuleRepository(RuleContext rulesContext, IMapper mapper)
        {
            _rulesContext = rulesContext;
            _mapper = mapper;
        }
        public async Task<List<RuleModel>> getRules(Guid loanProviderId)
        {//select roots of the tree that contain all their generation
            List<Rule> rulesList = await _rulesContext.Rule
                  .Include(r => r.ChildrenRules)
                  .Include(r => r.ParentRule)
                  .Where(r => r.LoanProviderId == loanProviderId).ToListAsync();
            List<RuleModel> rulesListModel = _mapper.Map<List<RuleModel>>(rulesList);
            return rulesListModel;
        }

        public List<RuleParameterModel> GetRulesParameterTypesDict()
        {
            List<RulesParameter> rulesParameter = _rulesContext.RulesParameters.ToList();
            return _mapper.Map<List<RuleParameterModel>>(rulesParameter);
        }
    }
}
