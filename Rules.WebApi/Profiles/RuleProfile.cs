using AutoMapper;
using Rules.Service.Models;
using Rules.WebApi.DTO;

namespace Rules.WebApi.Profiles
{
    public class RuleProfile : Profile
    {
        public RuleProfile()
        {
            CreateMap<RegisterDTO, RegisterModel>();
        }
    }
}
