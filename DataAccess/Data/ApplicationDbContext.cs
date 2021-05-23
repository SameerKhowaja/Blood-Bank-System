using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BloodDonor> BloodDonors { get; set; }
        public DbSet<BloodRecipient> BloodRecipients { get; set; }
        public DbSet<DonationRecord> DonationRecords { get; set; }
        public DbSet<ReceptionRecord> ReceptionRecords { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
    }
}
