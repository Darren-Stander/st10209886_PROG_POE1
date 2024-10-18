using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;

public class ClaimContext : DbContext
{
    public ClaimContext(DbContextOptions<ClaimContext> options) : base(options)
    {
    }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<ClaimFile> ClaimFiles { get; set; } // Ensure this line is here
}
/////////////////////////////////////////////////END OF FILE/////////////////////////////////////////////////
