using AutoMapper;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Loans.WebApi.DTO;
using Messages.Commands;
using Messages.MessagesOjects;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Loans.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loansService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public LoanController(ILoanService loansService, IMapper mapper, IMessageSession messageSession)
        {
            _loansService = loansService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost]
        public async Task<Guid> Create([FromBody] LoanDTO loan)//לבדוק שהמיפוי אכן עובד ולראות אם לקבל יד
        {            
            LoanModel loanModel = _mapper.Map<LoanModel>(loan);            
            Guid loanId = await _loansService.Create(loanModel);
            CheckLoanValid loanToCheck = new CheckLoanValid();
            LoanDetails loanDetails = _mapper.Map<LoanDetails>(loanModel);
            loanDetails.LoanId = loanId;
            loanToCheck.LoanDetails = loanDetails;
            await _messageSession.Send(loanToCheck)
                .ConfigureAwait(false);//del api
            return loanId;            
        }

        [HttpPut("{LoanId}")]
        public async Task Update(Guid loanId, [FromBody] LoanDTO loanToUpdate)//לבדוק שהמיפוי אכן עובד ולראות אם לקבל יד
        {
            LoanModel loanModel = _mapper.Map<LoanModel>(loanToUpdate);
            Guid _loanId = await _loansService.Update(loanId, loanModel);//האם צריך שיחזור היד
            CheckLoanValid loanToCheck = _mapper.Map<CheckLoanValid>(loanModel);
            loanToCheck.LoanDetails.LoanId = loanId;//לבדוק שאכן מאותחל בערך
            await _messageSession.Send(loanToCheck)
                .ConfigureAwait(false);
        }
    }
}
