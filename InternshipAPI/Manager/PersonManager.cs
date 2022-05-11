﻿using InternshipAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            Person personToUpdate = personsWithThatEmail.First(c => c.Password.Equals(oldPassword));
            if (personToUpdate == null) return null;
            personToUpdate.Password = newPassword;
            _context.Entry(personToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return personToUpdate;
        }
    }
}
