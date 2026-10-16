using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;

namespace CarInsurance.Database
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Insuree> Insurees { get; set; }
    }
}