using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class GroupOfPeopleContext : DbContext
    {
        public GroupOfPeopleContext(DbContextOptions<GroupOfPeopleContext> options) : base(options)
        {

        }
        public DbSet<GroupOfPeople> GroupOfPeople { get; set; }
    }
}
