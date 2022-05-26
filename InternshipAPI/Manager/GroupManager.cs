using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            _context.Group.Remove(group);
            _context.SaveChanges();
            return group;
        }
    }
}
