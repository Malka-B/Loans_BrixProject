using AutoMapper;
using Loans.Service.Models;
using Loans.WebApi.DTO;
using Messages.MessagesOjects;
using System;

namespace Loans.WebApi.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {            
            CreateMap<LoanDTO, LoanModel>();
            CreateMap<LoanModel, LoanDetails>()
                .ForMember(destination => destination.Age, option => option.MapFrom(src =>
                (DateTime.Now.Year - src.BirthDate.Year)));
            CreateMap<ApproveRuleDTO, ApproveRuleModel>();
        }

    }
}
