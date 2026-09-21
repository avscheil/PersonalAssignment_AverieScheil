using Microsoft.EntityFrameworkCore;

namespace PersonalAssignment_AverieScheil.Models
{
    public class TheContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserToData> UserToDatas { get; set; }
        public DbSet<DataRecord> DataRecords { get; set; }

        public TheContext(DbContextOptions options) : base(options)
        {
        
        }
    }
}
