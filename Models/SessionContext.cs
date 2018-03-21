using Microsoft.EntityFrameworkCore;

namespace VideoStoreApi.Models
{
    public class SessionContext : DbContext
    {
        public SessionContext(DbContextOptions<SessionContext> options) : base(options)
        {}

        public DbSet<EmployeeInfoToShare> Employees { get; set; }
    }
}
