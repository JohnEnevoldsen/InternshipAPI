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
    public class PersonManagerTests
    {
        private readonly PersonContext _context;
        PersonManager personManager;
        public PersonManagerTests()
        {
            DbContextOptionsBuilder<PersonContext> options = new DbContextOptionsBuilder<PersonContext>();
            options.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new PersonContext(options.Options);
        }

        [TestInitialize]
        public void StartTest()
        {
            personManager = new PersonManager(_context);
        }

        [TestMethod()]
        public void PersonManagerGetAllTest()
        {
            //Man kan ikke rigtig teste en GetAll metode
            List<Person> allPeople = personManager.GetAllPersons().ToList();
            foreach(var p in allPeople)
            {
                Assert.IsInstanceOfType(p, typeof(Person));
            }
        }
        [TestMethod()]
        public void PersonManagerGetOneTest()
        {
            //Jeg ved at John har id 7. I tillæg til det, så ved jeg at email og kodeord er henholdsvis john@.dk og to.
            Person contextPerson = _context.Person.Find(7);

            //Dette er en collection af en person
            IEnumerable<Person> managerPerson = personManager.GetOnePerson("john@.dk", "to");

            Assert.AreEqual(contextPerson.Id, managerPerson.First().Id);
            Assert.AreEqual(contextPerson.Name, managerPerson.First().Name);
            Assert.AreEqual(contextPerson.Mail, managerPerson.First().Mail);
            Assert.AreEqual(contextPerson.Password, managerPerson.First().Password);
            Assert.AreEqual(contextPerson.TelephoneNumber, managerPerson.First().TelephoneNumber);
            Assert.AreEqual(contextPerson.Internship, managerPerson.First().Internship);
            Assert.AreEqual(contextPerson.School, managerPerson.First().School);
            Assert.AreEqual(contextPerson.Role, managerPerson.First().Role);
        }
        [TestMethod()]
        public void PersonManagerUpdatePasswordTest()
        {
            //Denne og den oven over virker kun med min lokale database - John
            Person johnMedNytKodeord = personManager.UpdatePassword("john@.dk", "to", "tre");
            Assert.AreEqual("tre", johnMedNytKodeord.Password);

            Person johnMedDetGamleKodeord = personManager.UpdatePassword("john@.dk", "tre", "to");
            Assert.AreEqual("to", johnMedDetGamleKodeord.Password);
        }
        [TestMethod()]
        public void PersonManagerAddTest()
        {
            //Denne burde virke på alle computere
            Person data = new Person {
                Name = "Test John",
                Mail = "Test@.dk",
                Password = "EnToTre",
                TelephoneNumber = "31210101",
                Internship = "Et sted i Danmark",
                School = "Zealand, Roskilde",
                Role = "Bruger"
            };
            Person newPerson = personManager.AddPerson(data);
            Assert.IsTrue(newPerson.Id > 0);
            Assert.AreEqual(data.Id, newPerson.Id);
            Assert.AreEqual(data.Name, newPerson.Name);
            Assert.AreEqual(data.Mail, newPerson.Mail);
            Assert.AreEqual(data.Password, newPerson.Password);
            Assert.AreEqual(data.TelephoneNumber, newPerson.TelephoneNumber);
            Assert.AreEqual(data.Internship, newPerson.Internship);
            Assert.AreEqual(data.School, newPerson.School);
            Assert.AreEqual(data.Role, newPerson.Role);

            Person nullModelData = new Person();
            Assert.ThrowsException<OurDataBaseException>(() => personManager.AddPerson(nullModelData));
        }
        [TestMethod()]
        public void PersonManagerDeleteTest()
        {
            Person personToAdd = new Person
            {
                Name = "Slet",
                Mail = "Slette",
                Password = "Sletter",
                TelephoneNumber = "Slettede",
                Internship = "Slettet",
                School = "Delete",
                Role = "Deleted"
            };

            Person personToDelete = personManager.AddPerson(personToAdd);

            int personToDeleteId = personToDelete.Id;

            Person deletedPerson = personManager.DeletePerson(personToDeleteId);

            Assert.AreEqual(personToDeleteId, deletedPerson.Id);
            Assert.AreEqual("Slet", deletedPerson.Name);
            Assert.AreEqual("Slette", deletedPerson.Mail);
            Assert.AreEqual("Sletter", deletedPerson.Password);
            Assert.AreEqual("Slettede", deletedPerson.TelephoneNumber);
            Assert.AreEqual("Slettet", deletedPerson.Internship);
            Assert.AreEqual("Delete", deletedPerson.School);
            Assert.AreEqual("Deleted", deletedPerson.Role);
            Assert.IsNull(_context.Person.Find(personToDeleteId));

            IEnumerable<Person> deleteThisOneToo = personManager.GetOnePerson("Test@.dk", "EnToTre");

            int testId = deleteThisOneToo.First().Id;

            Person newDeletedPerson = personManager.DeletePerson(testId);
            Assert.AreEqual(testId, newDeletedPerson.Id);
            Assert.AreEqual("Test John", newDeletedPerson.Name);
            Assert.AreEqual("Test@.dk", newDeletedPerson.Mail);
            Assert.AreEqual("EnToTre", newDeletedPerson.Password);
            Assert.AreEqual("31210101", newDeletedPerson.TelephoneNumber);
            Assert.AreEqual("Et sted i Danmark", newDeletedPerson.Internship);
            Assert.AreEqual("Zealand, Roskilde", newDeletedPerson.School);
            Assert.AreEqual("Bruger", newDeletedPerson.Role);
        }
        [TestMethod()]
        public void PersonManagerUpdateTest()
        {
            //Denne virker også kun på min
            Person updates = new Person
            {
                Name = "Test John1",
                Mail = "Test@.dk1",
                Password = "EnToTreFire",
                TelephoneNumber = "31210102",
                Internship = "Et sted i Danmark og jeg fik syv",
                School = "Zealand, Roskilde, den bedste skole",
                Role = "Admin"
            };
            Person updatedPerson = personManager.UpdatePerson(7, updates);
            Assert.AreEqual(7, updatedPerson.Id);
            Assert.AreEqual(updates.Name, updatedPerson.Name);
            Assert.AreEqual(updates.Mail, updatedPerson.Mail);
            Assert.AreEqual(updates.Password, updatedPerson.Password);
            Assert.AreEqual(updates.TelephoneNumber, updatedPerson.TelephoneNumber);
            Assert.AreEqual(updates.Internship, updatedPerson.Internship);
            Assert.AreEqual(updates.School, updatedPerson.School);
            Assert.AreEqual(updates.Role, updatedPerson.Role);
            updates.Mail = "john@.dk";
            updates.Password = "to";
            updatedPerson = personManager.UpdatePerson(7, updates);
            Assert.AreEqual("john@.dk", updatedPerson.Mail);
            Assert.AreEqual("to", updatedPerson.Password);
        }
    }
}