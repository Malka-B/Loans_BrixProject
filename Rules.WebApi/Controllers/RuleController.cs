using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using Rules.WebApi.DTO;
using System;
using System.Threading.Tasks;

namespace Rules.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService _rulesService;
        private readonly IMapper _mapper;

        public RuleController(IRuleService rulesService, IMapper mapper)
        {
            _rulesService = rulesService;
            _mapper = mapper;
        }
        //פוסט לרישום והכנסת חוקים למערכת
        [HttpPost]
        public async Task RegisterToProvideLoans(RegisterDTO register)
        {
            RegisterModel registerModel = _mapper.Map<RegisterModel>(register);
            await _rulesService.RegisterToProvideLoans(registerModel);   
             
        }

        [HttpPost("update")]
        public async Task UpdatePolicyRules(RegisterDTO register)
        {
            RegisterModel registerModel = _mapper.Map<RegisterModel>(register);
            await _rulesService.UpdatePolicyRules(registerModel);            
        }
    }
}
