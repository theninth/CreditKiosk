using CreditKiosk.Models;
using System.Collections.Generic;
using System.Linq;

namespace CreditKiosk.Managers
{
    /// <summary>
    /// Manage basic database operations for the Person table.
    /// </summary>
    class PersonManager
    {
        /// <summary>
        /// Add Person.
        /// </summary>
        /// <param name="person">Person object to be added</param>
        public void Add(Person person)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(person);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes Person.
        /// </summary>
        /// <param name="person">Person object to be removed.</param>
        public void Remove(Person person)
        {
            using (var context = new KioskDbContext())
            {
                context.Persons.Remove(person);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all persons, sorted by first name.
        /// </summary>
        /// <returns>Persons sorted by first name.</returns>
        public List<Person> GetAllSorted()
        {
            using (var context = new KioskDbContext())
            {
                return context.Persons.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
            }
        }
    }
}
