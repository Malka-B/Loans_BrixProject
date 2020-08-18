using Rules.Service.Interfaces;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rule.Service.Models
{
    public static class ParameterType
    {

        public static Dictionary<string, string> SetRulesParameterTypes(this ICheckRuleRepository checkRulesRepository)
        {
            List<RuleParameterModel> dictionary = checkRulesRepository.GetRulesParameterTypesDict();
            Dictionary<string, string> RulesParameterTypes = new Dictionary<string, string>();
            foreach (var d in dictionary)
            {
                RulesParameterTypes.Add(d.ParameterName, d.ParameterType);
            }
            return RulesParameterTypes;
        }
    }
}
