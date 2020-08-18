using AutoMapper;
using Loans.Data.Entities;
using Loans.Service.Models;

namespace Loans.Data.Profiles
{
    public class LoanRepositoryProfile : Profile
    {
        public LoanRepositoryProfile()
        {
            CreateMap<LoanModel, LoanEntity>();
            CreateMap<LoanEntity, LoanModel>();
            CreateMap<LoanFailureRuleModel, LoanFailureRulesEntity>();
            CreateMap<LoanFailureRulesEntity, LoanFailureRuleModel>();            
        }
    }
}
