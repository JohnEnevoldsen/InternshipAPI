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
        //Når man skal opdatere en persons status, brug dette
        public ActivityStatus Update(int id, string statusToChangeTo)
        {
            ActivityStatus activityStatusToUpdate = _context.ActivityStatus.Find(id);
            Status status = _context.Status.Where(c => c.MyStatus.Equals(statusToChangeTo)).FirstOrDefault();
            activityStatusToUpdate.StatusId = status.Id;
            _context.Entry(activityStatusToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return activityStatusToUpdate;
        }
        public IEnumerable<ActivityStatus> GetAllActivityStatuses()
        {
            return _context.ActivityStatus;
        }
    }
}
