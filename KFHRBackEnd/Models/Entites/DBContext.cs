using Microsoft.EntityFrameworkCore;
using System;

namespace KFHRBackEnd.Models.Entites
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<LateMinutesLeft> LateMinutesLeft { get; set; }
        public DbSet<RecommendedCertificate> RecommendedCertificates { get; set; }
        public DbSet<EarnedReward> EarnedRewards { get; set; }
    }


}
