using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class GroupContext : DbContext
    {
        public GroupContext(DbContextOptions<GroupContext> options) : base(options)
        {

        }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupOfPeople> GroupOfPeople { get; set; }
        public DbSet<ActivityAndGroupsOfPeople> ActivityAndGroupsOfPeople { get; set; }
    }
}
