using AutoMapper;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Loans.WebApi.DTO;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManagerLoanController : ControllerBase
    {
        private readonly IManagerLoanService _managerLoansService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public ManagerLoanController(IManagerLoanService managerLoansService, IMapper mapper, IMessageSession messageSession)
        {
            _managerLoansService = managerLoansService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost("{loanId}")]
        public async Task ApproveLoan(Guid loanId, [FromBody]List<ApproveRuleDTO> approveRules)
        {            
            ApproveLoanModel approveLoanModel = _mapper.Map<ApproveLoanModel>(approveRules);
            approveLoanModel.LoanId = loanId;
            CheckLoanValid checkLoanValid = await _managerLoansService.ApproveLoan(approveLoanModel);                       
            await _messageSession.Send(checkLoanValid)
                .ConfigureAwait(false);            
        }
    }
}
