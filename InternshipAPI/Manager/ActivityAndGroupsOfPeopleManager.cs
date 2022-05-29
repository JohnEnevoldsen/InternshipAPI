using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InternshipAPI.Manager
{
    public class ActivityAndGroupsOfPeopleManager
    {
        private readonly ActivityAndGroupsOfPeopleContext _context;
        public ActivityAndGroupsOfPeopleManager(ActivityAndGroupsOfPeopleContext context)
        {
            _context = context;
        }
        public ActivityAndGroupsOfPeopleManager()
        {

        }
        public IEnumerable<ActivityAndGroupsOfPeople> GetAllActivityAndGroupsOfPeople()
        {
            return _context.ActivityAndGroupsOfPeople;
        }
        public IEnumerable<ActivityAndGroupsOfPeople> addGroupToActivity(int activityId, int groupOfPeopleId)
        {
            try
            {
                IEnumerable<GroupOfPeople> groupOfPeople = _context.GroupOfPeople.Where(c => c.GroupId.Equals(groupOfPeopleId));
                List<ActivityAndGroupsOfPeople> activityAndGroupsOfPeople = new List<ActivityAndGroupsOfPeople>();
                foreach (var groupOfPeopleRow in groupOfPeople)
                {
                    ActivityAndGroupsOfPeople activityAndGroupOfPeopleToAdd = new ActivityAndGroupsOfPeople { ActivityId = activityId, GroupsOfPeopleId = groupOfPeopleRow.Id };
                    activityAndGroupsOfPeople.Add(activityAndGroupOfPeopleToAdd);
                }
                _context.AddRange(activityAndGroupsOfPeople);
                _context.SaveChanges();
                return activityAndGroupsOfPeople;
            }
            catch (DbUpdateException ex)
            {
                throw new OurDataBaseException(ex.InnerException.Message);
            }
        }
    }
}
