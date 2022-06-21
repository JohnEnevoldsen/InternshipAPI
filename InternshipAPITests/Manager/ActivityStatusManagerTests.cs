using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipAPI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InternshipAPI.Models;

namespace InternshipAPI.Manager.Tests
{
    [TestClass()]
    public class ActivityStatusManagerTests
    {
        private readonly ActivityStatusContext _context;
        ActivityStatusManager activityStatusManager;

        public ActivityStatusManagerTests()
        {
            DbContextOptionsBuilder<ActivityStatusContext> options = new DbContextOptionsBuilder<ActivityStatusContext>();
            options.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new ActivityStatusContext(options.Options);
        }

        [TestInitialize]
        public void StartTest()
        {
            activityStatusManager = new ActivityStatusManager(_context);
        }
        [TestMethod()]
        public void ActivityStatusManagerGetAllTest()
        {
            List<ActivityStatus> activityStatuses = activityStatusManager.GetAllActivityStatuses().ToList();
            foreach(var item in activityStatuses)
            {
                Assert.IsInstanceOfType(item, typeof(ActivityStatus));
            }
        }
        [TestMethod()]
        public void ActivityStatusManagerGetAllByActivityTest()
        {
            List<ActivityStatus> activityStatuses = activityStatusManager.GetAllByActivity(1).ToList();
            foreach (var item in activityStatuses)
            {
                Assert.IsInstanceOfType(item, typeof(ActivityStatus));
            }
        }
        [TestMethod()]
        public void ActivityStatusManagerGetByActivityAndPersonIdTest()
        {
            ActivityStatus activityStatus = activityStatusManager.GetByActivityAndPersonId(1, 7);
            Assert.AreEqual(4, activityStatus.Id);
            Assert.AreEqual(1, activityStatus.ActivityId);
            Assert.AreEqual(7, activityStatus.StatusId);
            Assert.IsNull(activityStatusManager.GetByActivityAndPersonId(27, 27));
        }
        [TestMethod()]
        public void ActivityStatusManagersAddActivityStatusTest()
        {
            DbContextOptionsBuilder<ActivityContext> options1 = new DbContextOptionsBuilder<ActivityContext>();
            options1.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true");
            ActivityContext activityContext = new ActivityContext(options1.Options);
            ActivityManager activityManager = new ActivityManager(activityContext);

            DbContextOptionsBuilder<PersonContext> options2 = new DbContextOptionsBuilder<PersonContext>();
            options2.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true");
            PersonContext personContext = new PersonContext(options2.Options);
            PersonManager personManager = new PersonManager(personContext);

            Person dataPerson = new Person
            {
                Name = "Activity Status John",
                Mail = "Test@.dk",
                Password = "EnToTreFireFem",
                TelephoneNumber = "31210101",
                Internship = "Et sted i Danmark",
                School = "Zealand, Roskilde",
                Role = "Bruger"
            };
            Person newPerson = personManager.AddPerson(dataPerson);

            Activity dataActivity = new Activity
            {
                Date = DateTime.Now,
                Headline = "Test activity"
            };
            Activity newActivity = activityManager.AddActivity(dataActivity);

            ActivityStatus newActivityStatus = activityStatusManager.AddActivityStatus(newActivity.Id, newPerson.Id);
            Assert.IsTrue(newActivityStatus.Id > 0);
            Assert.AreEqual(newActivityStatus.ActivityId, newActivity.Id);

            IEnumerable<Status> statusesWithThatPersonId = _context.Status.Where(c => c.PersonId.Equals(newPerson.Id));
            Status statusToAdd = statusesWithThatPersonId.First(c => c.MyStatus.Equals("I have not answered"));

            Assert.AreEqual(newActivityStatus.StatusId, statusToAdd.Id);
            Assert.ThrowsException<InvalidOperationException>(() => activityStatusManager.AddActivityStatus(4, 1));
        }
        [TestMethod()]
        public void ActivityStatusUpdateTest()
        {
            ActivityStatus updatedActivityStatus = activityStatusManager.Update(4, 7, "I am attending");
            Assert.AreEqual(6, updatedActivityStatus.StatusId);
            updatedActivityStatus = activityStatusManager.Update(4, 7, "I am not attending");
            Assert.AreEqual(7, updatedActivityStatus.StatusId);
        }
    }
}