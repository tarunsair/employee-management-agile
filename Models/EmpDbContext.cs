using Microsoft.EntityFrameworkCore;

namespace employee_management_agile.Models
{
    public class EmpDbContext : DbContext
    {
        public EmpDbContext(DbContextOptions<EmpDbContext> options) : base(options)
        {
        }

        public DbSet<EmpModel> EmployeeTable { get; set; }
    }
}