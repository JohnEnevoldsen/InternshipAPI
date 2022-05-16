using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class ActivityStatusContext : DbContext
    {
        public ActivityStatusContext(DbContextOptions<ActivityStatusContext> options) : base(options)
        {

        }

        public DbSet<ActivityStatus> ActivityStatus { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
