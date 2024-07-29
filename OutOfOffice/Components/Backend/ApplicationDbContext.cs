using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using OutOfOffice.Components.Data;
using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.Components.Backend
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long, IdentityUserClaim<long>, IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<EmployeeData> EmployyData { get; set; }
        public DbSet<ProjectData> ProjectData { get; set; }
        public DbSet<LeaveRequestData> LeaveRequestData { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<ApprovalRequestData> ApprovalRequestData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeData>()
            .HasOne(b => b.ProjectInformation)
            .WithMany(w => w.EmployeesList)
            .HasForeignKey(w => w.ProjectId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
