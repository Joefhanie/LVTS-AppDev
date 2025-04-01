using LVTS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LVTS.Data
{
    public class LVTSContext : IdentityDbContext
    {
        public LVTSContext(DbContextOptions<LVTSContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; } = null!;
    }
}
