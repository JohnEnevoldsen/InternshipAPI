using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class StatusContext : DbContext
    {
        public StatusContext(DbContextOptions<StatusContext> options) : base(options)
        {

        }

        public DbSet<Status> Status { get; set; }
    }
}
