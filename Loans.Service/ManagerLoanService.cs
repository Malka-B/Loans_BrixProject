using AutoMapper;
using Exceptions;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Messages.Commands;
using Messages.MessagesOjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loans.Service
{
    public class ManagerLoanService : IManagerLoanService
    {
        private readonly IManagerLoanRepository _managerLoansRepository;
        private readonly IMapper _mapper;

        public ManagerLoanService(IManagerLoanRepository managerLoansRepository, IMapper mapper)
        {
            _managerLoansRepository = managerLoansRepository;
            _mapper = mapper;
        }
        public async Task<CheckLoanValid> ApproveLoan(ApproveLoanModel approveLoanModel)
        {
            bool isLoanExist = await _managerLoansRepository.CheckLoanExist(approveLoanModel.LoanId);
            if (!isLoanExist)
            {
                throw new LoanNotFoundException();               
            }
            await ApproveFailureRulesInDB(approveLoanModel);
            CheckLoanValid checkLoanValid = await CreateCheckLoanValid(approveLoanModel); 
            return checkLoanValid;
        }

        private async Task<CheckLoanValid> CreateCheckLoanValid(ApproveLoanModel approveLoanModel)
        {
            List<int> ignoreRules = new List<int>();
            foreach(var r in approveLoanModel.approveRules)
            {
                ignoreRules.Add(r.ApproveRuleId);
            }
            LoanModel loanModel = await _managerLoansRepository.GetLoanById(approveLoanModel.LoanId);
            CheckLoanValid checkLoanValid = new CheckLoanValid();//האם המיםוי של יד נצרך
            LoanDetails loanDetails = _mapper.Map<LoanDetails>(loanModel);
            checkLoanValid.LoanDetails = loanDetails;
            checkLoanValid.IgnoreRules = ignoreRules;
            checkLoanValid.LoanDetails.LoanId = approveLoanModel.LoanId;
            return checkLoanValid;
        }

        private async Task ApproveFailureRulesInDB(ApproveLoanModel approveLoanModel)
        {
            List<LoanFailureRuleModel> loanFailureRules = await GetFailureRulesByLoanId(approveLoanModel.LoanId);
            ApproveFailureRulesDetails(loanFailureRules, approveLoanModel.approveRules);
            await _managerLoansRepository.UpdateApproveFailureRules(approveLoanModel.LoanId,loanFailureRules);
        }

        private void ApproveFailureRulesDetails(List<LoanFailureRuleModel> loanFailureRules, List<ApproveRuleModel> approveRules)
        {
            foreach (var approveRule in approveRules)
            {
                LoanFailureRuleModel loanFailureRule = loanFailureRules
                    .FirstOrDefault(l => l.RuleId == approveRule.ApproveRuleId);
                loanFailureRule.ManagerComment = approveRule.ManagerComment;
                loanFailureRule.ManagerSignature = approveRule.ManagerSignature;
            }
        }

        private async Task<List<LoanFailureRuleModel>> GetFailureRulesByLoanId(Guid loanId)
        {
            return await _managerLoansRepository.GetFailureRulesByLoanId(loanId);
        }
    }
}
