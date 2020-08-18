using AutoMapper;
using Loans.Service.Models;
using Messages.MessagesOjects;
using System;

namespace Loans.Service.Profiles
{
    public class LoanServiceProfile : Profile
    {
        public LoanServiceProfile()
        {
            CreateMap<RuleTreeNode, LoanFailureRuleModel>();
            CreateMap<LoanModel, LoanDetails>()
               .ForMember(destination => destination.Age, option => option.MapFrom(src =>
               (DateTime.Now.Year - src.BirthDate.Year)));
        }      
    }
}
