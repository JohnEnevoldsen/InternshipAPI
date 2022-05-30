using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InternshipAPI.Manager
{
    public class GroupManager
    {
        private readonly GroupContext _context;

        public GroupManager(GroupContext context)
        {
            _context = context;
        }
        public GroupManager()
        {

        }
        public IEnumerable<Group> GetAllGroups()
        {
            return _context.Group;
        }
        public Group AddGroup(Group newGroup)
        {
            try
            {
                _context.Group.Add(newGroup);
                _context.SaveChanges();
                return newGroup;
            }
            catch (DbUpdateException ex)
            {
                _context.Group.Remove(newGroup);
                throw new OurDataBaseException(ex.InnerException.Message);

            }
        }
        public Group DeleteGroup(int id)
        {
            Group group = _context.Group.Find(id);
            if (group == null) return null;
            List<GroupOfPeople> groupOfPeoples = _context.GroupOfPeople.Where(x => x.GroupId == group.Id).ToList();
            List<ActivityAndGroupsOfPeople> activityAndGroupsOfPeople = new List<ActivityAndGroupsOfPeople>();
            foreach (var groupOfPeopleRow in groupOfPeoples)
            {
                List<ActivityAndGroupsOfPeople> activityAndGroupsOfPeoplesWhereGroupOfPeople = _context.ActivityAndGroupsOfPeople.Where(x => x.GroupsOfPeopleId == groupOfPeopleRow.Id).ToList();
                foreach (var item in activityAndGroupsOfPeoplesWhereGroupOfPeople)
                {
                    activityAndGroupsOfPeople.Add(item);
                }
            }
            _context.RemoveRange(activityAndGroupsOfPeople);
            _context.SaveChanges();
            _context.RemoveRange(groupOfPeoples);
            _context.SaveChanges();
            _context.Group.Remove(group);
            _context.SaveChanges();
            return group;
        }
    }
}
