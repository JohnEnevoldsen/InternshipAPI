using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class ActivityContext : DbContext
    {
        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options)
        {

        }

        public DbSet<Activity> Activity { get; set; }
    }
}
