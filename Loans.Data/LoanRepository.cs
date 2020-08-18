using AutoMapper;
using Enums;
using Loans.Data.Entities;
using Loans.Service.Interfaces;
using Loans.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loans.Data
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanContext _loansContext;
        private readonly IMapper _mapper;

        public LoanRepository(LoanContext loansContext, IMapper mapper)
        {
            _loansContext = loansContext;
            _mapper = mapper;
        }

        public async Task<bool> CheckLoanExist(Guid loanId)
        {
            LoanEntity loanEntity = await _loansContext.Loans
                .FirstOrDefaultAsync(l => l.ID == loanId);
            if (loanEntity == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Guid> Create(LoanModel loanModel)
        {
            //האם לבדוק אם ספק ההלוואה קיים?//מסתמכת על ספקים קבועים במאגר
            LoanEntity loanEntity = _mapper.Map<LoanEntity>(loanModel);
            loanEntity.ID = Guid.NewGuid();
            loanEntity.LoanDate = DateTime.Now;
            await _loansContext.AddAsync(loanEntity);
            await _loansContext.SaveChangesAsync();
            return loanEntity.ID;
        }

        public async Task<Guid> Update(Guid loanId, LoanModel loanModel)
        {
            LoanEntity loanEntity = _mapper.Map<LoanEntity>(loanModel);
            LoanEntity loanToUpdate = await _loansContext.Loans
                .FirstOrDefaultAsync(l => l.ID == loanId);
            loanToUpdate.LoanAmount = loanEntity.LoanAmount;
            loanToUpdate.Salary = loanEntity.Salary;
            loanToUpdate.FixedSalary = loanEntity.FixedSalary;
            loanToUpdate.LoanPurpose = loanEntity.LoanPurpose;
            loanToUpdate.NumberOfRepayments = loanEntity.NumberOfRepayments;
            loanToUpdate.SeniorityYearsInBank = loanEntity.SeniorityYearsInBank;
            loanToUpdate.LoanStatus = loanEntity.LoanStatus;
            await _loansContext.SaveChangesAsync();
            return loanToUpdate.ID;
        }

        public async Task UpdateLoanFailureRules(Guid loanId, List<LoanFailureRuleModel> failureRules)
        {
            LoanEntity loanToUpdateFailureRules = await _loansContext.Loans
                 .FirstOrDefaultAsync(l => l.ID == loanId);
            List<LoanFailureRulesEntity> loanFailureRules = _mapper.Map<List<LoanFailureRulesEntity>>(failureRules);
            loanToUpdateFailureRules.FailureRules = new List<LoanFailureRulesEntity>();
            loanToUpdateFailureRules.FailureRules.AddRange(loanFailureRules);
            await _loansContext.SaveChangesAsync();
        }

        public async Task UpdateLoanStatus(Guid loanId, LoanStatus loanStatus)
        {
            LoanEntity loanToUpdateStatus = await _loansContext.Loans
                 .FirstOrDefaultAsync(l => l.ID == loanId);
            loanToUpdateStatus.LoanStatus = loanStatus;
            await _loansContext.SaveChangesAsync();
        }
    }
}
