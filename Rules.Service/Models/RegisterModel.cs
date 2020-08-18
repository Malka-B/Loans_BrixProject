using System;

namespace Rules.Service.Models
{
    public class RegisterModel
    {
        public string FilePath { get; set; }
        public Guid ProviderLoanId { get; set; }
    }
}
