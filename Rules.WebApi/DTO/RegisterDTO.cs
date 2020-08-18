using System;

namespace Rules.WebApi.DTO
{
    public class RegisterDTO
    {
        public string FilePath { get; set; }
        public Guid ProviderLoanId { get; set; }
    }
}
