using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InternshipAPI.Manager
{
    public class ActivityStatusManager
    {
        private readonly ActivityStatusContext _context;
        public ActivityStatusManager(ActivityStatusContext context)
        {
            _context = context;
        }
        public ActivityStatusManager()
        {

        }
        //Når man tilføjer en person til en aktivitet, brug dette
        public ActivityStatus AddActivityStatus(int activityId, int personId)
        {
            ActivityStatus newActivityStatus = new ActivityStatus();
            try
            {
                newActivityStatus.ActivityId = activityId;
                IEnumerable<Status> statusesWithThatPersonId = _context.Status.Where(c => c.PersonId.Equals(personId));
                Status statusToAdd = statusesWithThatPersonId.First(c => c.MyStatus.Equals("I have not answered"));
                newActivityStatus.StatusId = statusToAdd.Id;
                _context.ActivityStatus.Add(newActivityStatus);
                _context.SaveChanges();
                return newActivityStatus;
            }
            catch (DbUpdateException ex)
            {
                _context.ActivityStatus.Remove(newActivityStatus);
                throw new OurDataBaseException(ex.InnerException.Message);

            }
        }
        public ActivityStatus Update(string statusToChangeTo, int activityId, int personId)
        {
            IEnumerable<Status> statusesWithThatPersonId = _context.Status.Where(c => c.PersonId.Equals(personId));

            IEnumerable<ActivityStatus> activityStatuses = _context.ActivityStatus.Where(c => c.ActivityId.Equals(activityId));
            
            Status statusIdToChangeTo = statusesWithThatPersonId.First(c => c.MyStatus.Equals(statusToChangeTo));

            ActivityStatus activityStatusToUpdate = new ActivityStatus();

            activityStatusToUpdate = activityStatuses.First(c => c.Equals(statusesWithThatPersonId));

            activityStatusToUpdate.StatusId = statusIdToChangeTo.Id;

            _context.Entry(activityStatusToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return activityStatusToUpdate;
        }
    }
}
