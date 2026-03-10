using Microsoft.EntityFrameworkCore;
using employee_management_agile.Models;

namespace employee_management_agile.Models
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
        {
        }

        public DbSet<LoginModel> LoginsTable { get; set; }
    }
}