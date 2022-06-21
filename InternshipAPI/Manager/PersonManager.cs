using InternshipAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace InternshipAPI.Manager
{
    public class PersonManager
    {
        private readonly PersonContext _context;
        public PersonManager(PersonContext context)
        {
            _context = context;
        }
        public PersonManager()
        {

        }
        public IEnumerable<Person> GetAllPersons()
        {
            return _context.Person;
        }
        public IEnumerable<Person> GetOnePerson(string email, string password)
        {
            return _context.Person.Where(c => c.Mail.Equals(email) && c.Password.Equals(password));
        }
        public Person UpdatePassword(string email, string oldPassword, string newPassword)
        {
            IEnumerable<Person> personsWithThatEmail = _context.Person.Where(c => c.Mail.Equals(email));
            Person personToUpdate = null;
            try
            {
               personToUpdate = personsWithThatEmail.First(c => c.Password.Equals(oldPassword));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            
            personToUpdate.Password = newPassword;
            _context.Entry(personToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return personToUpdate;
        }
        public Person AddPerson(Person newPerson)
        {
            try
            {
                _context.Person.Add(newPerson);
                _context.SaveChanges();
                try
                {
                    List<Status> statuses = new List<Status>
                    {
                        new Status{PersonId = newPerson.Id, MyStatus ="I am attending"},
                        new Status{PersonId = newPerson.Id, MyStatus ="I am not attending"},
                        new Status{PersonId = newPerson.Id, MyStatus ="I have not answered"}
                    };
                    _context.AddRange(statuses);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new OurDataBaseException(ex.InnerException.Message);
                }
                return newPerson;
            }
            catch (DbUpdateException ex)
            {
                _context.Person.Remove(newPerson);
                throw new OurDataBaseException(ex.InnerException.Message);

            }
        }
        public Person DeletePerson(int id)
        {
            Person person = _context.Person.Find(id);
            if (person == null) return null;
            List<Status> statuses = _context.Status.Where(x => x.PersonId == person.Id).ToList();
            List<GroupOfPeople> groupOfPeoples = _context.GroupOfPeople.Where(x => x.PersonId == id).ToList();
            List<ActivityAndGroupsOfPeople> activityAndGroupsOfPeople = new List<ActivityAndGroupsOfPeople>();
            foreach (var groupOfPeopleRow in groupOfPeoples)
            {
                List<ActivityAndGroupsOfPeople> activityAndGroupsOfPeoplesWhereGroupOfPeople = _context.ActivityAndGroupsOfPeople.Where(x => x.GroupsOfPeopleId == groupOfPeopleRow.Id).ToList();
                foreach (var item in activityAndGroupsOfPeoplesWhereGroupOfPeople)
                {
                    activityAndGroupsOfPeople.Add(item);
                }
            }
            List<ActivityStatus> activityStatusesToDelete = new List<ActivityStatus>();
            foreach(var status in statuses)
            {
                List<ActivityStatus> activityStatuses = _context.ActivityStatus.Where(x => x.StatusId == status.Id).ToList();
                foreach(var item in activityStatuses)
                {
                    activityStatusesToDelete.Add(item);
                }
            }
            _context.RemoveRange(activityAndGroupsOfPeople);
            _context.SaveChanges();
            _context.RemoveRange(groupOfPeoples);
            _context.SaveChanges();
            _context.RemoveRange(activityStatusesToDelete);
            _context.SaveChanges();
            _context.RemoveRange(statuses);
            _context.SaveChanges();
            _context.Person.Remove(person);
            _context.SaveChanges();
            return person;
        }

        public Person UpdatePerson(int id, Person updates)
        {
            try
            {
                Person person = _context.Person.Find(id);
                if (person == null) return null;
                person.Name = updates.Name;
                person.Mail = updates.Mail;
                person.Password = updates.Password;
                person.TelephoneNumber = updates.TelephoneNumber;
                person.Internship = updates.Internship;
                person.School = updates.School;
                person.Role = updates.Role;
                _context.Entry(person).State = EntityState.Modified;
                _context.SaveChanges();
                return person;
            }
            catch (DbUpdateException ex)
            {
                throw new OurDataBaseException(updates + " " + ex.InnerException.Message);
            }
        }
        public Person GetPersonById(int id)
        {
            return _context.Person.Find(id);
        }
    }
}
