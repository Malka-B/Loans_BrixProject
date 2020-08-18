using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using Rules.Service.Interfaces;
using System.Threading.Tasks;

namespace Rules.NSB
{
    public class CheckLoanValidHandler : IHandleMessages<CheckLoanValid>
    {
        private readonly ICheckRuleService _checkRulesService;        
        static ILog log = LogManager.GetLogger<CheckLoanValidHandler>();

        public CheckLoanValidHandler(ICheckRuleService checkRulesService)
        {
            _checkRulesService = checkRulesService;           
        }

        public async Task Handle(CheckLoanValid message, IMessageHandlerContext context)
        {
            LoanChecked loanChecked = await _checkRulesService.CheckLegality(message);
            log.Info($"in CheckLoanLegalityHandler loanId : {message.LoanDetails.LoanId}");            
            await context.Publish(loanChecked)
                .ConfigureAwait(false);   
        }
    }
}
