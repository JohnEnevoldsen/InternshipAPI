using InternshipAPI.Models;
using System.Collections.Generic;

namespace InternshipAPI.Manager
{
    public class StatusManager
    {
        private readonly StatusContext _context;
        public StatusManager(StatusContext context)
        {
            _context = context;
        }
        public StatusManager()
        {

        }
        public IEnumerable<Status> GetAllStatuses()
        {
            return _context.Status;
        }
        public Status GetStatusById(int id)
        {
            return _context.Status.Find(id);
        }
    }
}
