using Loans.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loans.Data
{
    public class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions<LoanContext> options) : base(options)
        {

        }

        public LoanContext()
        { }

        public DbSet<LoanEntity> Loans { get; set; }
        public DbSet<LoanFailureRulesEntity> FailureRules { get; set; }
              
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanEntity>()
                .HasMany(l => l.FailureRules)
                .WithOne(e => e.Loan);
        }
    }
}
