using Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Loans.WebApi.DTO
{
    public class LoanDTO
    {
        [Required]
        public int BorrowerID { get; set; }
        [Required]
        public Guid LoanProviderID { get; set; }
        [Required]
        [Range(1, 1000000)]
        public int Salary { get; set; }
        [Required]
        public bool FixedSalary { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [Range(1, 1000000)]
        public int LoanAmount { get; set; }
        [Required]
        [Range(1, 100)]
        public int SeniorityYearsInBank { get; set; }
        [Required]
        [Range(1,500)]
        public int NumberOfRepayments { get; set; }
        [Required]
        [Range(0, 3)]//enum with 4 values
        public LoanPurpose LoanPurpose { get; set; }
    }
}
