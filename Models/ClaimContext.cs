using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;

public class ClaimContext : IdentityDbContext
{
    public ClaimContext(DbContextOptions<ClaimContext> options) : base(options)
    {
    }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<ClaimFile> ClaimFiles { get; set; } // Ensure this line is here
    public DbSet<Lecturer> Lecturers { get; set; } // Add DbSet for Lecturer
}
/////////////////////////////////////////////////END OF FILE/////////////////////////////////////////////////
