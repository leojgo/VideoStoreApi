using Microsoft.EntityFrameworkCore;

namespace VideoStoreApi.Models
{
    public class PermissionContext : DbContext
    {
        public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
        {}

        public DbSet<Permission> Permissions { get; set; }
    }
}
