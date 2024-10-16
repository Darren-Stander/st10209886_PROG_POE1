using Microsoft.EntityFrameworkCore;

namespace st10209886_PROG_POE1.Models
{
    public class ClaimContext : DbContext
    {
        public ClaimContext(DbContextOptions<ClaimContext> options) : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
    }
}
