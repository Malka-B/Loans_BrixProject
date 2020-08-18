using AutoMapper;
using Messages.Events;
using Messages.MessagesOjects;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rules.Service.Profiles
{
    public class RuleServiceProfile : Profile
    {
        public RuleServiceProfile()
        {
            CreateMap<RuleModel, RuleTreeNode>()
                .ForMember(destination => destination.ParentRuleId, option => option.MapFrom(src =>
                src.ParentRule.Id))
                .ForMember(destination => destination.RuleId, option => option.MapFrom(src =>
                src.Id));
        }
    }
}
