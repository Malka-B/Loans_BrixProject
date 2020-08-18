using System.ComponentModel.DataAnnotations;

namespace Loans.WebApi.DTO
{
    public class ApproveRuleDTO
    {
        [Required]
        public int ApproveRuleId { get; set; }
        [Required]
        public string ManagerComment { get; set; }
        [Required]
        public string ManagerSignature { get; set; }
    }
}
