using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalAssignment_AverieScheil.Data;

namespace PersonalAssignment_AverieScheil.Models
{
    public class TheContext : IdentityDbContext<ApplicationUser>
    {
        public TheContext(DbContextOptions<TheContext> options) : base(options)
        {

        }

        public DbSet<UserToData> UserToDatas { get; set; }
        public DbSet<DataRecord> DataRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
