using Microsoft.EntityFrameworkCore;
using InternshipAPI.Models;

namespace InternshipAPI.Manager
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {

        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ActivityStatus> ActivityStatus { get; set; }
        public DbSet<GroupOfPeople> GroupOfPeople { get; set; }
        public DbSet<ActivityAndGroupsOfPeople> ActivityAndGroupsOfPeople { get; set; }
    }
}
