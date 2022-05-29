using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InternshipAPI.Manager
{
    public class GroupOfPeopleManager
    {
        private readonly GroupOfPeopleContext _context;

        public GroupOfPeopleManager(GroupOfPeopleContext context)
        {
            _context = context;
        }
        public GroupOfPeopleManager()
        {

        }
        public GroupOfPeople AddPersonToGroup(int groupId, int personId)
        {
            GroupOfPeople newGroupOfPeople = new GroupOfPeople();
            try
            {
                newGroupOfPeople.GroupId = groupId;
                newGroupOfPeople.PersonId = personId;
                _context.GroupOfPeople.Add(newGroupOfPeople);
                _context.SaveChanges();
                return newGroupOfPeople;
            }
            catch (DbUpdateException ex)
            {
                _context.GroupOfPeople.Remove(newGroupOfPeople);
                throw new OurDataBaseException(ex.InnerException.Message);

            }
        }
        public IEnumerable<GroupOfPeople> GetAllGroupsOfPeople()
        {
            return _context.GroupOfPeople;
        }
    }
}
