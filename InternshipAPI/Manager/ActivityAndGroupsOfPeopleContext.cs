using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class ActivityAndGroupsOfPeopleContext : DbContext
    {
        public ActivityAndGroupsOfPeopleContext(DbContextOptions<ActivityAndGroupsOfPeopleContext> options) : base(options)
        {
            
        }
        public DbSet<ActivityAndGroupsOfPeople> ActivityAndGroupsOfPeople { get; set; }
        public DbSet<GroupOfPeople> GroupOfPeople { get; set; }
    }
}
