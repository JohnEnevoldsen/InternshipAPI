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
        public Person Update(string email, string oldPassword, string newPassword)
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
            List<Status> statuses = new List<Status>
                    {
                        new Status{PersonId = id, MyStatus ="I am attending"},
                        new Status{PersonId = id, MyStatus ="I am not attending"},
                        new Status{PersonId = id, MyStatus ="I have not answered"}
                    };
            _context.RemoveRange(statuses);
            _context.Person.Remove(person);
            _context.SaveChanges();
            return person;
        }
    }
}
