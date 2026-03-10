using Microsoft.EntityFrameworkCore;
using employee_management_agile.Models;

namespace employee_management_agile.Models
{
    public class EmpDbContext : DbContext
    {
        public EmpDbContext(DbContextOptions<EmpDbContext> options) : base(options)
        {
        }

        public DbSet<EmpModel> EmployeesTable { get; set; }
    }
}