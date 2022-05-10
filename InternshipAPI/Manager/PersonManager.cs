using InternshipAPI.Models;
using System.Collections.Generic;
using System.Linq;

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
    }
}
