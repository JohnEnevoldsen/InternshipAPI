using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InternshipAPI.Manager
{
    public class ActivityManager
    {
        private readonly ActivityContext _context;
        public ActivityManager(ActivityContext context)
        {
            _context = context;
        }
        public ActivityManager()
        {

        }
        public IEnumerable<Activity> GetAllActivities()
        {
            return _context.Activity;
        }
        public Activity GetActivityById(int id)
        {
            return _context.Activity.Find(id);
        }
        public Activity AddActivity(Activity newActivity)
        {
            try
            {
                _context.Activity.Add(newActivity);
                _context.SaveChanges();
                return newActivity;
            }
            catch (DbUpdateException ex)
            {
                _context.Activity.Remove(newActivity);
                throw new OurDataBaseException(ex.InnerException.Message);

            }
        }
        public IEnumerable<Activity> GetAllActivitiesWithPersonId(int personId)
        {
            List<Activity> activitiesToReturn = new List<Activity>();
            List<int> activitiesId = new List<int>();
            foreach(var item in _context.ActivityStatus)
            {
                if(_context.Status.Find(item.StatusId).PersonId == personId)
                {
                    activitiesId.Add(item.ActivityId);
                }
            }
            foreach (int id in activitiesId)
            {
                activitiesToReturn.Add(_context.Activity.Find(id));
            }
            return activitiesToReturn;
        }
    }
}
