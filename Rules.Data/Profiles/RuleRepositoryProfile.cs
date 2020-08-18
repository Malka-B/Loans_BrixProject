using AutoMapper;
using Rules.Data.Entities;
using Rules.Service.Models;

namespace Rules.Data.Profiles
{
    public class RuleRepositoryProfile : Profile
    {
        public RuleRepositoryProfile()
        {
            CreateMap<Rule, RuleModel>();
            CreateMap<RulesParameter, RuleParameterModel>();
            CreateMap<RuleModel, Rule>()               
                .ForMember(destination => destination.Id , option => option.Ignore()); 
        }
    }
}
