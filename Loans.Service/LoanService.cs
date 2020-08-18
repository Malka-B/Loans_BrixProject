using AutoMapper;
using Enums;
using Exceptions;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Messages.Events;
using Messages.MessagesOjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loans.Service
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loansRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loansRepository, IMapper mapper)
        {
            _loansRepository = loansRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Create(LoanModel loanModel)
        {
            loanModel.LoanStatus = LoanStatus.InProcess;
            Guid loanId = await _loansRepository.Create(loanModel);

            return loanId;
        }

        public async Task<Guid> Update(Guid loanId, LoanModel loanModel)
        {
            bool isLoanExist = await _loansRepository.CheckLoanExist(loanId);
            if (isLoanExist)
            {
                loanModel.LoanStatus = LoanStatus.InProcess;
                return await _loansRepository.Update(loanId, loanModel);
            }
            else
            {
                return await Create(loanModel);
            }

        }

        public async Task UpdateLoanAfterCheckLegality(LoanChecked message)
        {
            bool isLoanExist = await _loansRepository.CheckLoanExist(message.LoanId);
            if (isLoanExist)
            {
                if (message.IsLoanValid)
                {
                    await UpdateSucceded_ValidLoan(message.LoanId);
                }
                else
                {
                    await UpdateFailed_InvalidLoan(message);
                }
            }
            else
            {
                throw new LoanNotFoundException();
            }
        }

        private async Task UpdateFailed_InvalidLoan(LoanChecked message)
        {
            await _loansRepository.UpdateLoanStatus(message.LoanId, LoanStatus.FailedInvalid);
            List<RuleTreeNode> failureRulesNode = GetFailureRulesList(message.CheckedRuleTree);
            List<LoanFailureRuleModel> failureRules = _mapper.Map<List<LoanFailureRuleModel>>(failureRulesNode);
            await _loansRepository.UpdateLoanFailureRules(message.LoanId, failureRules);
        }

        private List<RuleTreeNode> GetFailureRulesList(List<RuleTreeNode> checkedRuleTree)
        {
            return checkedRuleTree.Where(ruleNode => ruleNode.IsRuleValid == false).ToList();
        }

        private async Task UpdateSucceded_ValidLoan(Guid loanId)
        {
            await _loansRepository.UpdateLoanStatus(loanId, LoanStatus.SucceededValid);
        }
    }
}
