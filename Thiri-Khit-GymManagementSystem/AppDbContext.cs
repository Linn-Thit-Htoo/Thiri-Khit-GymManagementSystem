using Microsoft.EntityFrameworkCore;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserDataModel> Users { get; set; }
        public DbSet<MembershipPlanDataModel> MembershipPlan { get; set; }
        public DbSet<MembershipDataModel> Membership { get; set; }
    }
}
